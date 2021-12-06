using System.IO;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    public class RoomSaverLoader : MonoBehaviour
    {
        public void LoadRoom(string roomID)
        {
            CreatorNetworkUtility.LoadEventFromAddress("https://xrchitecture.de/upload/" + roomID + "/ExampleEvent.json");
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
            CreatorNetworkUtility.SaveCurrentEventToAddress("https://xrchitecture.de/upload/" + roomID + "/ExampleEvent.json");
        }
        
        public void SaveRoom(bool local,string roomlocation)
        {
            
            XrEventContainer containerToUpload =
                CreatorSessionManager.GetCreatorEvent();

            string jsonToUpload =
                XrJsonUtility.ParseJsonFromEvent(containerToUpload);
            
            
            File.WriteAllText(roomlocation, jsonToUpload);

        }

        public void newRoom(string guid)
        {
            XrEventContainer _event = new XrEventContainer();
            CreatorSessionManager.SetCreatorEvent(_event);
            
        }
    }
}
