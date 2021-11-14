using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;

public class WegGLConnection : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject spherePrefab;
    [SerializeField] private GameObject pointLight;
    [SerializeField] private Material defaultMaterial;
    
    
    public void SpawnItemEvent (string type) {
        switch (type)
        {
            case "Cube":
                GameObject cube = Instantiate(cubePrefab);
                cube.GetComponent<Renderer>().material = defaultMaterial;
                break;
            case "Sphere":
                GameObject sphere = Instantiate(spherePrefab);
                sphere.GetComponent<Renderer>().material = defaultMaterial;
                break;
            case "Lamp":
                GameObject light = Instantiate(pointLight);
                break;
            default:
                Debug.Log("no Object specified");
                break;
        }
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
