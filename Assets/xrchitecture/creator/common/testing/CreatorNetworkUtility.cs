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
                    PostRoomJson(address, jsonToUpload));
            }
        }


        internal static IEnumerator GetRoomJson(string address,
            Action<string> onSuccess)
        {
            WebClient request = new WebClient();
            request.Credentials = TestConfigHelper.FtpCredentials;
            UnityWebRequest req = UnityWebRequest.Get(address);

            req.SendWebRequest();

            int fileSize = 0;

            while (!req.isDone)
            {
                yield return new WaitForSeconds(.1f);
            }

            onSuccess(req.downloadHandler.text);
        }

        internal static IEnumerator PostRoomJson(string address, string jsonData,
            Action onSuccess = null)
        {
 
            byte[] bytes = Encoding.UTF8.GetBytes(jsonData);
            
            using (UnityWebRequest www = UnityWebRequest.Put(address, bytes))
            {
                www.SetRequestHeader("Content-Type", "application/json");
                www.method = "POST";
                yield return www.Send();
 
                if (www.isError)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }

            if (onSuccess != null)
            {
                onSuccess();
            }
        }
    }
}