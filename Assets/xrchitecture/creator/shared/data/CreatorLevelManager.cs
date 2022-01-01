using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;

[RequireComponent(typeof(persistenceManager))]
[RequireComponent(typeof(HelperBehaviour))]
public class CreatorLevelManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> prefabList;
    [SerializeField] private CreatorControlerScript creatorControlerScript;


    //for safety Reasons; this Object should always be at the Origin
    void Awake()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
        TestConfigHelper.PrefabList = prefabList;
        
        persistenceManager pm = this.GetComponent<persistenceManager>();
        if (!PlayerPrefs.HasKey("roomID"))
        {
            pm.createGUID();
            newRoom();
            ReportRoomID();
            return;
        }
        ReportRoomID();
        loadRoom(pm.getGUID());
    }

    private void ReportRoomID()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        
#if UNITY_WEBGL == true
        WebGLConnection wgl = GetComponent<WebGLConnection>();
        wgl.ReportRoomIdUnity(pm.getGUID());
#endif
#if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        ui.DisplayGuid(pm.getGUID());
#endif
    }


    //SPAWNING AND DELETING OBJECTS:
    public void SpawnPrefab(string type)
    {
        Debug.Log(type);
        CreatorSessionManager.SpawnItemInCurrentRoom(type, "pre-defined");
        Debug.Log($"Spawning {type}!");
    }


    public void SpawnGltf(string nameOnServer)
    {
        Debug.Log(nameOnServer);
        CreatorSessionManager.SpawnItemInCurrentRoom(nameOnServer, "user-defined");
    }

    public void DeleteSelectedItem()
    {
        GameObject objectToDelete = creatorControlerScript.selectedObject;
        CreatorSessionManager.RemoveItemFromCurrentRoom(objectToDelete.GetComponent<CreatorItem>());
    }


    //gets the deletedItemName from React 
    public void CustomItemDeletedFromServer(string itemName)
    {
        GameObject room = CreatorSessionManager.GetCurrentRommGameObject();
        foreach (CreatorItem item in room.GetComponentsInChildren<CreatorItem>())
        {
            if (item.name == itemName + "-ROOT")
            {
                CreatorSessionManager.RemoveItemFromCurrentRoom(item);
            }
        }
    }

   

    //GUID FUN:
    /*
     * This function requires the persistenceManager script
     */
    public void SetGUID(string guid)
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.setGUID(guid);
    }

    //LOADING + SAVING ROOMS:
    public void loadRoom(string guid)
    {
        SetGUID(guid);
        CreatorNetworkUtility.LoadEventFromAddress("https://xrchitecture.de/upload/" + guid + "/EventLayout.json", guid);
        Debug.Log("Unity loading room: " + guid);

        // Report back guid
        ReportRoomID();
    }

    public void SaveRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        string guid = pm.getGUID();

        CreatorNetworkUtility.SaveCurrentEventToAddress(guid);
        //ShowReactPopup("Save successful");
    }

    public void newRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.createGUID();
        CreatorSessionManager.CreateNewCreatorEvent(pm.getGUID());
        ReportRoomID();
    }

    public void SaveRoomLocal(string path)
    {
        XrEventContainer eventContainer = CreatorSessionManager.GetCreatorEvent();
                string jsonToUpload = XrJsonUtility.ParseJsonFromEvent(eventContainer);
                File.WriteAllText(path, jsonToUpload);
    }

    public void LoadRoomLocal(string path)
    {
        using StreamReader r = new StreamReader(path);
        string json = r.ReadToEnd();
        XrEventContainer eventContainer = XrJsonUtility.ParseEventFromJson(json);
        CreatorSessionManager.SetCreatorEvent(eventContainer);
    }
}
