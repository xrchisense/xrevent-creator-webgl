using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using Xrchitecture.Creator.Common.Data;


[RequireComponent(typeof(gltfImporter))]
[RequireComponent(typeof(persistenceManager))]
public class WebGLConnection : MonoBehaviour
{
    [SerializeField] public List<GameObject> PrefabList;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] public RoomSaverLoader roomSaverLoader;

    //for safety Reasons; this Object should always be at the Origin
    void Awake()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
    }
    
    public void SpawnPrefab(string type)
    {
        Debug.Log(type);
        GameObject gO = Instantiate(PrefabList.Find(x => x.name == type));
        Renderer y = new Renderer();
        if (gO.TryGetComponent<Renderer>(out y)) y.material = defaultMaterial;
        Debug.Log($"Spawning {type}!");
    }
    
    /*
     * This function requires the gltfImporter script
     */
    public void SpawnGltf(string url)
    {
        Debug.Log(url);
        gltfImporter imp = this.GetComponent<gltfImporter>();
        imp.importGltfFromServer(url);
    }


    /*
     * This function requires the persistenceManager script
     */
    public void SetGUID(string guid)
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.setGUID(guid);
    }


    public void loadRoom(string guid)
    {
        SetGUID(guid);
        // Call loading stuff here!
        roomSaverLoader.LoadRoom(guid);
        Debug.Log("Unityloading room: " + guid);

        // Report back guid
        ReportRoomIdUnity();
    }
    
    public void SaveRoom(string guid)
    {
        roomSaverLoader.SaveRoom(guid);
        //ShowReactPopup("Save successful");
    }

    public void newRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.createGUID();
        roomSaverLoader.newRoom(pm.getGUID());
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