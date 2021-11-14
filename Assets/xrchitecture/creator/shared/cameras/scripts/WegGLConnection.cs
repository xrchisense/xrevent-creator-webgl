using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WegGLConnection : MonoBehaviour
{
    
    
    public void SpawnItemEvent (string type) {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
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
