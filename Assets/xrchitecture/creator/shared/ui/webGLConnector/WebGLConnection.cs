using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(gltfImporter))]
[RequireComponent(typeof(persistenceManager))]
public class WebGLConnection : MonoBehaviour
{
    [SerializeField] public List<GameObject> PrefabList;

    [SerializeField] private Material defaultMaterial;

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

        // Report back guid
        reportRoomIdUnity();
    }

    /*
     * This function requires the persistenceManager script
     */
    public void newRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.createGUID();
        reportRoomIdUnity();
    }

    //
    //
    //   These are the messages sent via jslib plugin to the react app
    //
    // 
    [DllImport("__Internal")]
    private static extern void ItemInfo(string itemName, int itemID);
    private static extern void ShowPopup(string textStringPointer);
    private static extern void reportRoomID(string id);

    public void ItemInfoToWebGL(string itemName, int itemID)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ItemInfo (itemName, itemID);
        Debug.Log("send ItemInfo");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }

    public void ShowWebGLPopup(string textStringPointer)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ShowPopup (textStringPointer);
        Debug.Log("send ShowPopup");
        return;
#endif
        Debug.Log("NNOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }
    
    public void reportRoomIdUnity()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        persistenceManager pm = this.GetComponent<persistenceManager>();
        reportRoomID (pm.getGUID());
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send reportRoomIdUnity");
    }
}