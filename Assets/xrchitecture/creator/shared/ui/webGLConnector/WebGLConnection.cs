using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

/*
 * Dependencies: 
 * - gltfImporter.cs
 */

public class WebGLConnection : MonoBehaviour
{
    [SerializeField] 
    public List<GameObject> PrefabList;
    
    [SerializeField] 
    private Material defaultMaterial;
    
    public void SpawnPrefab (string type)
    {
        Debug.Log(type);
        GameObject gO = Instantiate(PrefabList.Find(x => x.name == type));
        Renderer y = new Renderer();
        if (gO.TryGetComponent<Renderer>(out y))y.material = defaultMaterial;
        Debug.Log ($"Spawning {type}!");
    }
    void Awake()
    {
        this.transform.position = new Vector3(0f, 0f, 0f); 
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
    
    
    // These are the messages sent via jslib plugin to the react app

    [DllImport("__Internal")]
    private static extern void SpawnItem(string type, int score);

    public void SpawnItemUnity(string type)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    SpawnItem (type, 100);
#endif
    }


    [DllImport("__Internal")]
    private static extern void reportRoomID(string id);
    public void reportRoomIdUnity()
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    persistenceManager pm = this.GetComponent<persistenceManager>();
    reportRoomID (pm.getGUID());
#endif
    }


}
