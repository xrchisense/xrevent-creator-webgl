using System.IO;
using System.Linq;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    public class RoomSaverLoader : MonoBehaviour
    {

        [SerializeField] private GameObject[] defaultItems;
        public void LoadRoom(string roomID)
        {
            CreatorNetworkUtility.LoadEventFromAddress("https://xrchitecture.de/upload/" + roomID + "/EventLayout.json");
        }
        public void LoadRoom(bool local, string roomlocation)
        {
            using (StreamReader r = new StreamReader(roomlocation))
            {
                string json = r.ReadToEnd();
                CreatorSessionManager.SetCreatorEvent(XrJsonUtility.ParseEventFromJson(json));
            }
        }

        public void SaveRoom(string roomID)
        {
            CreatorNetworkUtility.SaveCurrentEventToAddress("https://xrchitecture.de/upload/" + roomID + "/EventLayout.json");
        }
        
        public void SaveRoom(bool local,string roomlocation)
        {
            
            XrEventContainer containerToUpload =
                CreatorSessionManager.GetCreatorEvent();

            string jsonToUpload =
                XrJsonUtility.ParseJsonFromEvent(containerToUpload);
            
            
            File.WriteAllText(roomlocation, jsonToUpload);

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
            foreach (var gameObject in defaultItems)
            {
                Instantiate(gameObject);
            }
            //XrEventContainer _event = new XrEventContainer();
            //CreatorSessionManager.SetCreatorEvent(_event);
            
        }
    }
}
