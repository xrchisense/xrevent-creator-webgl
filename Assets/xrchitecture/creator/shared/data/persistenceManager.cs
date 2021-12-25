using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Xrchitecture.Creator.Common.Data;

/*
 * This class mainly helps with storing Playerprefs to the IndexedDB located in the Browser.
 * 
 */
public class persistenceManager : MonoBehaviour
{
    public void setGUID(string guid)
    {
        //TODO: The Testconfig helper is still needed for the GUID when loading user Objects (XrCreaterUtiliy.CreateUserGameObject)
        TestConfigHelper.ProjectId = guid;
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
