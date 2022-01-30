using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;

/// <summary>
/// This is an empty Singleton that exists to run Coroutines on it
/// </summary>
public class HelperBehaviour : MonoBehaviour
{
    public float currentJsonVersion => TestConfigHelper.CurrentJsonVersion;
    
    public CreatorLevelManager LevelManager => GetComponent<CreatorLevelManager>();

    public static HelperBehaviour Instance
    {
        
        get
        {
            if (_Instance == null)
            {
                _Instance = GameObject.FindObjectOfType<HelperBehaviour>();
            }

            return _Instance;
        }
    }

    public void OnFinishLoad()
    {
        
        GameObject roomRoot = CreatorSessionManager.GetCurrentRoomGameObject();
        foreach (Transform child in roomRoot.transform)
        {
            child.gameObject.SetActive(true);
        }
    }

    private static HelperBehaviour _Instance;
}
