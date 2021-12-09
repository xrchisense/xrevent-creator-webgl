using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is an empty Singleton that exists to run Coroutines on it
/// </summary>
public class HelperBehaviour : MonoBehaviour
{
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

    private static HelperBehaviour _Instance;
}
