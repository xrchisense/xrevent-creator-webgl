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
using System.Net;
using UnityEngine;
using UnityEngine.Networking;

namespace Xrchitecture.Creator.Common.Data
{
    public static class CreatorNetworkUtility
    {
        internal static void LoadEventFromAddress(string address)
        {
            HelperBehaviour.Instance.StartCoroutine(RoomLoadRoutine());
            
            IEnumerator RoomLoadRoutine()
            {
                yield return HelperBehaviour.Instance.StartCoroutine(GetRoomJson(address, json => CreatorEventManager.SetCreatorEvent(XrJsonUtility.ParseEventFromJson(json))));
            }
        }


        internal static IEnumerator GetRoomJson(string address, Action<string> onSuccess)
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
    }
}