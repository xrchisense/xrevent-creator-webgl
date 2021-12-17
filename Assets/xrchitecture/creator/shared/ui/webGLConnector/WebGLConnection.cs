using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;

[RequireComponent(typeof(persistenceManager))]
public class WebGLConnection : MonoBehaviour
{
    [SerializeField] public  List<GameObject> prefabList;
    [SerializeField] private CreatorControlerScript creatorControlerScript;
    [SerializeField] public RoomSaverLoader roomSaverLoader;
    
    

    //for safety Reasons; this Object should always be at the Origin
    void Awake()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
        TestConfigHelper.PrefabList = prefabList;
        
    }
    
    
    
    
    //SPAWNING AND DELETING OBJECTS:
    public void SpawnPrefab(string type)
    {
        Debug.Log(type);
        CreatorSessionManager.SpawnItemInCurrentRoom(type,"pre-defined");
        Debug.Log($"Spawning {type}!");
    }
    

    public void SpawnGltf(string nameOnServer)
    {
        Debug.Log(nameOnServer);
        CreatorSessionManager.SpawnItemInCurrentRoom(nameOnServer,"user-defined");
    }

    public void DeleteSelectedItem()
    {
        GameObject objectToDelete = creatorControlerScript.selectedObject;
        CreatorSessionManager.RemoveItemFromCurrentRoom(objectToDelete.GetComponent<CreatorItem>());
    }
    
    
    //gets the deletedItemName from React (Important!: Without the file Ending)
    public void CustomItemDeletedFromServer(string itemName) {
            GameObject room = CreatorSessionManager.GetCurrentRommGameObject();
            foreach (CreatorItem item in room.GetComponentsInChildren<CreatorItem>() {
                if (item.name == itemName + "-ROOT") {
                    CreatorSessionManager.RemoveItemFromCurrentRoom(item);
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
        // Call loading stuff here!
        roomSaverLoader.LoadRoom(guid);
        Debug.Log("Unity loading room: " + guid);

        // Report back guid
        ReportRoomIdUnity();
    }
    
    public void SaveRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        string guid = pm.getGUID();

        roomSaverLoader.SaveRoom(guid);
        //ShowReactPopup("Save successful");
    }

    public void newRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.createGUID();
        roomSaverLoader.NewRoom(pm.getGUID());
        ReportRoomIdUnity();
    }

    //
    //
    //   These are the messages sent via jslib plugin to the react app
    //
    // 
    [DllImport("__Internal")]
    private static extern void ItemInfo(string itemName, int itemID);
    [DllImport("__Internal")]
    private static extern void ShowPopup(string textStringPointer);
    [DllImport("__Internal")]
    private static extern void ReportRoomID(string id);

    public void ItemInfoToWebGL(string itemName, int itemID)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ItemInfo (itemName, itemID);
        Debug.Log("Untiy did send ItemInfo");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }

    public void ShowReactPopup(string textStringPointer)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ShowPopup (textStringPointer);
        Debug.Log("Unity did send ShowPopup");
        return;
#endif
        Debug.Log("NNOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }
    
    public void ReportRoomIdUnity()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        Debug.Log("UnityRoomID: " + pm.getGUID());
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        
        ReportRoomID (pm.getGUID());
        Debug.Log("Unity did send RoomID to Webgl");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send reportRoomIdUnity");
    }
}