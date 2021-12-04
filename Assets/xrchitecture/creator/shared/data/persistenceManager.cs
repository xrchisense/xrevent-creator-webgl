using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*
 * This class mainly helps with storing Playerprefs to the IndexedDB located in the Browser.
 * 
 */
public class persistenceManager : MonoBehaviour
{
    
    public WebGLConnection webGlConnection;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("roomID"))
        {
            createGUID();
        }
            webGlConnection.reportRoomIdUnity();
    }

    public void setGUID(string guid)
    {
        PlayerPrefs.SetString("roomID", guid.ToString());
    }
    public string getGUID()
    {
        return PlayerPrefs.GetString("roomID");
    }

    public void createGUID()
    {
        Guid myGUID = Guid.NewGuid();
        PlayerPrefs.SetString("roomID", myGUID.ToString());
    }
    
}
