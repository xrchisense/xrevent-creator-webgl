using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;


[RequireComponent(typeof(gltfImporter))]
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
     * This Function requires the gltfImporter script
     */
    public void SpawnGltf(string url)
    {
        Debug.Log(url);
        gltfImporter imp = this.GetComponent<gltfImporter>();
        imp.importGltfFromServer(url);
    }
    

    [DllImport("__Internal")]
    private static extern void ItemInfo(string itemName, int itemID);
    private static extern void ShowPopup(string textStringPointer);

    public void ItemInfoToWebGL(string itemName,int itemID)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ItemInfo (itemName, itemID);
        Debug.Log("send ItemInfo");
        return;
#endif
        Debug.Log("NOT WEBGL: DID NOT send ItemInfo");
    }
    
    public void ShowWebGLPopup(string textStringPointer)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ShowPopup (textStringPointer);
        Debug.Log("send ShowPopup");
        return;
#endif
        Debug.Log("NOT WEBGL: DID NOT send ShowPopup");
    }

}
