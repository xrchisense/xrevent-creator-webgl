using System;
using System.Collections;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace Xrchitecture.Creator.Common.Data
{
    internal class TestJson : MonoBehaviour
    {
        public void LoadTestRoom()
        {
            StartCoroutine(RoomLoadRoutine());
            
            
            IEnumerator RoomLoadRoutine()
            {
                yield return StartCoroutine(GetRoomJson("ExampleRoom", json => RoomCreatorUtility.CreateRoomGameObject(ParseRoomFromJson(json))));
            }
        }

        internal IEnumerator GetRoomJson(string roomId, Action<string> onSuccess)
        {
            WebClient request = new WebClient();
            request.Credentials = TestConfigHelper.FtpCredentials;
            string url = TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId + "/rooms/" + roomId + ".json";

            UnityWebRequest req = UnityWebRequest.Get(url);

            req.SendWebRequest();

            int fileSize = 0;

            while (!req.isDone)
            {
                yield return new WaitForSeconds(.1f);
            }

            onSuccess(req.downloadHandler.text);
        }

        private RoomContainer ParseRoomFromJson(string roomString)
        {
            RoomContainer roomContainer = JsonConvert.DeserializeObject<RoomContainer>(roomString);
            return roomContainer;
        }
    }
}