using System;
using UnityEngine;

public class TestJson : MonoBehaviour
{
    [SerializeField]
    private GameObject gameObject; 
    private void Start()
    {
        string jsonGo = JsonUtility.ToJson(gameObject.GetComponent<Renderer>());
        print(jsonGo);
    }
}

