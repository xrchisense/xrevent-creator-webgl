using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal partial class TestJson : MonoBehaviour
    {
        [SerializeField]
        private GameObject gameObject;

        private void Start()
        {
            Vector3 pos = new Vector3(1, 0, 0);
            Quaternion rot = new Quaternion(1, 0, 0, 1);

            //string jsonGo = JsonUtility.ToJson(rot);
            string roomJson = GetRoomJson("ExampleRoom");

            Room roomToBuild = ParseRoomFromJson(roomJson);
            
            RoomCreatorUtility.CreateRoomGameObject(roomToBuild);
        }

        internal string GetRoomJson(string roomId)
        {
            WebClient request = new WebClient();
            request.Credentials = TestConfigHelper.FtpCredentials;
            string url = TestConfigHelper.UserDataFolder +
                         TestConfigHelper.UserId +
                         "/Rooms/" + roomId + ".json";

            byte[] fileData = request.DownloadData(url);

            string roomJson = System.Text.Encoding.UTF8.GetString(fileData);

            return roomJson;
        }

        private Room ParseRoomFromJson(string roomString)
        {
            Room room = JsonConvert.DeserializeObject<Room>(roomString);
            return room;
        }
    }
}