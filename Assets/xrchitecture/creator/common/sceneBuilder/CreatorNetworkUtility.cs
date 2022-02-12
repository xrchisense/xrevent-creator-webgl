using System;
using System.Collections;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace Xrchitecture.Creator.Common.Data
{
    public static class CreatorNetworkUtility
    {
        
        internal static void LoadEventFromAddress(string address,string guid)
        {
            HelperBehaviour.Instance.StartCoroutine(EventLoadRoutine());

            IEnumerator EventLoadRoutine()
            {
                yield return HelperBehaviour.Instance.StartCoroutine(
                    GetRoomJson(address, guid,
                       json => LoadRoomFromJson(json,guid) ));
            }
        }

        //<Summary>
        //Used to fetch the json string from the webserver.  
        //</Summary>  
        internal static IEnumerator GetRoomJson(string address,string guid, Action<string> onSuccess)
        {
            UnityWebRequest req = UnityWebRequest.Get(address);
            req.SendWebRequest();

            while (!req.isDone)
            {
                yield return new WaitForSeconds(.1f);
            }

            
            if (req.downloadHandler.data != null && req.downloadHandler.text[0] != '<' )
            {
                onSuccess(req.downloadHandler.text);
            }
            else
            {
                //Room could not be loaded Error Message!
                Debug.Log("Recieved Data is HTML?! This means the Room is not available on the Server!");
                
                //create new Room:
                CreatorSessionManager.CreateNewCreatorEvent(guid);
            }

        }

        internal static void LoadRoomFromJson(string json, string guid)
        {
            XrEventContainer xrEvent;
            try
            {
                xrEvent = XrJsonUtility.ParseEventFromJson(json);
                CreatorSessionManager.SetCreatorEvent(xrEvent);
            }
            catch (Exception e)
            {
                //Room could not be loaded Error Message!
                Debug.Log("Could not parse to Room Event: " + e);
                
                //create new Room:
                CreatorSessionManager.CreateNewCreatorEvent(guid);
            }
            
        }

        internal static void SaveCurrentEventToAddress(string roomID)
        {
            HelperBehaviour.Instance.StartCoroutine(EventSaveRoutine());

            IEnumerator EventSaveRoutine()
            {
                XrEventContainer containerToUpload = CreatorSessionManager.GetCreatorEvent();
                
                //TODO THis is working but a stupid way of doing it !! ALL ITEMS GET DELELTED BEFORE SAVING!!!!!
                //QuickFixTOGettAllTheCurrentItems (only needed because Transofrms is not updated in the Event HEHE LOL
                containerToUpload.Rooms[0].Items = new List<ItemContainer>();
                foreach (var cI in CreatorSessionManager.GetCurrentRoomGameObject().GetComponentsInChildren<CreatorItem>())
                {
                    cI.TransformUpdated();
                    cI.CustomParametersUpdated();
                    containerToUpload.Rooms[0].Items.Add(cI.ItemContainer);
                }
                
                
                string jsonToUpload = XrJsonUtility.ParseJsonFromEvent(containerToUpload);
                
                yield return HelperBehaviour.Instance.StartCoroutine(
                    PostRoomJson(roomID, jsonToUpload));
            }
        }

        internal static IEnumerator PostRoomJson(string roomID, string jsonData, Action onSuccess = null)
        {
            /* ToDo: Should use the MultipartForm to upload to the .php endpoint the 
             * below use is deprecated.
            List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            formData.Add(new MultipartFormFileSection(jsonData, "EventLayout.json"));
            formData.Add(new MultipartFormDataSection("folder=" + roomID ));
            */

            // This is the deprecated way to do HTTP-Post requests
            WWWForm form = new WWWForm();
            form.AddField("file", jsonData);
            form.AddField("folder", roomID);

            UnityWebRequest www = UnityWebRequest.Post("https://xrchitecture.de/unityUploader.php", form);
           
            yield return www.SendWebRequest();
            
            // ToDo: Needs som better error handling. Also including the result of the .php endpoint
            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            } else {
                Debug.Log("Upload complete!");
            }
            Debug.Log(www.downloadHandler.text); // response from server upload php
            if (onSuccess != null)
            {
                onSuccess();
            }

            yield return null;
        }
    }
}