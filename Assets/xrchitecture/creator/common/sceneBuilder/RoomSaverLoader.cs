using System.IO;
using System.Linq;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    public class RoomSaverLoader : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] defaultItems;
        
        public void LoadRoom(bool local, string path)
        {
            using StreamReader r = new StreamReader(path);
            string json = r.ReadToEnd();
            XrEventContainer eventContainer = XrJsonUtility.ParseEventFromJson(json);
            CreatorSessionManager.SetCreatorEvent(eventContainer);
        }  
        
        public void SaveRoom(bool local,string path)
        {
            XrEventContainer eventContainer = CreatorSessionManager.GetCreatorEvent();
            string jsonToUpload = XrJsonUtility.ParseJsonFromEvent(eventContainer);           
            File.WriteAllText(path, jsonToUpload);
        }

        public void LoadRoom(string roomID)
        {
            CreatorNetworkUtility.LoadEventFromAddress("https://xrchitecture.de/upload/" + roomID + "/EventLayout.json");
        }

        public void SaveRoom(string roomID)
        {
            CreatorNetworkUtility.SaveCurrentEventToAddress(roomID);
        }

        public void NewRoom(string guid)
        {
            GameObject[] spawnedItemsList;
            spawnedItemsList = GameObject.FindGameObjectsWithTag("EventItem");
            foreach (var g in spawnedItemsList)
            {
                Destroy(g);
            }
            //Spawn Default Items:
           CreatorSessionManager.CreateNewCreatorEvent(guid);
        }
    }
}
