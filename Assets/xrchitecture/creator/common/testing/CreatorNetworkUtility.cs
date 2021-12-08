// ------------------------------------------------------------------------------------------------------
// <copyright file="CreatorNetworkUtility.cs" company="Monstrum">
// Copyright (c) 2021 Monstrum. All rights reserved.
// </copyright>
// <author>
//   Nikolai Reinke
// </author>
// ------------------------------------------------------------------------------------------------------

using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace Xrchitecture.Creator.Common.Data
{
    public static class CreatorNetworkUtility
    {
        internal static void LoadEventFromAddress(string address)
        {
            HelperBehaviour.Instance.StartCoroutine(EventLoadRoutine());

            IEnumerator EventLoadRoutine()
            {
                yield return HelperBehaviour.Instance.StartCoroutine(
                    GetRoomJson(address,
                        json => CreatorSessionManager.SetCreatorEvent(
                            XrJsonUtility.ParseEventFromJson(json))));
            }
        }

        internal static void SaveCurrentEventToAddress(string address)
        {
            HelperBehaviour.Instance.StartCoroutine(EventSaveRoutine());

            IEnumerator EventSaveRoutine()
            {
                XrEventContainer containerToUpload =
                    CreatorSessionManager.GetCreatorEvent();

                string jsonToUpload =
                    XrJsonUtility.ParseJsonFromEvent(containerToUpload);

                yield return HelperBehaviour.Instance.StartCoroutine(
                    PutRoomJson(address, jsonToUpload));
            }
        }


        internal static IEnumerator GetRoomJson(string address,
            Action<string> onSuccess)
        {
            UnityWebRequest req = UnityWebRequest.Get(address);

            req.SendWebRequest();

            int fileSize = 0;

            while (!req.isDone)
            {
                yield return new WaitForSeconds(.1f);
            }

            onSuccess(req.downloadHandler.text);
        }

        internal static IEnumerator PutRoomJson(string address, string jsonData,
            Action onSuccess = null)
        {

            WebClient client = new WebClient();
            
            byte[] myData = System.Text.Encoding.UTF8.GetBytes(jsonData);
            UnityWebRequest www = UnityWebRequest.Put(address, myData);
            yield return www.SendWebRequest();
 
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                Debug.Log("Upload complete!");
            }
            if (onSuccess != null)
            {
                onSuccess();
            }

            yield return null;
        }
    }
}