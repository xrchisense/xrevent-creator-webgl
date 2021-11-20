using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class WegGLConnection : MonoBehaviour
{
    [SerializeField] public List<GameObject> PrefabList;

    
    
    [SerializeField] private Material defaultMaterial;
    
    
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
    
    
    
    
    
    
    


    [DllImport("__Internal")]
    private static extern void SpawnItem(string type, int score);

    public void SpawnItemUnity(string type)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
    SpawnItem (type, 100);
#endif
    }

}
