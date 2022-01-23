using System;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;
using Random = System.Random;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(persistenceManager))]
[RequireComponent(typeof(HelperBehaviour))]
public class CreatorLevelManager : MonoBehaviour
{
    [SerializeField] public List<GameObject> prefabList;
    public List<Material> SkyBoxList;
    private List<ItemContainer> defaultItemsList;
    
    [SerializeField] private List<GameObject> defaultGameObjectsList;
    [SerializeField] private List<Vector3> defaultGameObjectsPositions;
    
    [SerializeField] private CreatorControlerScript creatorControlerScript;
    [CanBeNull] private Action<int> popUpActionWhenClicked = null;

    //for safety Reasons; this Object should always be at the Origin
    void Awake()
    {
        this.transform.position = new Vector3(0f, 0f, 0f);
        TestConfigHelper.PrefabList = prefabList;
        
        persistenceManager pm = this.GetComponent<persistenceManager>();
        if (!PlayerPrefs.HasKey("roomID"))
        {
            pm.createGUID();
            newRoom();
            ReportRoomID();
            return;
        }
        ReportRoomID();
        loadRoom(pm.getGUID());


        defaultItemsList = new List<ItemContainer>();
        int u = 0;
        foreach (var x in defaultGameObjectsList)
        {
            defaultItemsList.Add(new ItemContainer(){ItemType = "pre-defined",ResourceName = x.name,Position = defaultGameObjectsPositions[u]});
            u += 1;
        }

#if !UNITY_EDITOR && UNITY_WEBGL
        // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keabord inputs
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }

  
    private void ReportRoomID()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        
#if UNITY_WEBGL == true
        WebGLConnection wgl = GetComponent<WebGLConnection>();
        wgl.ReportRoomIdUnity(pm.getGUID());
#endif
#if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        ui.DisplayGuid(pm.getGUID());
#endif
    }

    
    //POUP Managment
    public void ShowPopUp(string titelString, string bodyTextString, string button1Text, Action<int> onPopUpClick, string button2Text = "null", string button3Text = "null", bool showX = false)
    {
#if UNITY_WEBGL == true
        popUpActionWhenClicked = onPopUpClick;
        WebGLConnection wgl = GetComponent<WebGLConnection>();
        wgl.ShowReactPopup(titelString, bodyTextString, button1Text, button2Text, button3Text, showX);
#endif
#if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        //ui.DisplayGuid(pm.getGUID());
#endif
    }

    public void PopUpFeedback(int buttonNumber)
    {
        if (popUpActionWhenClicked == null) return;
        
        popUpActionWhenClicked(buttonNumber);
        popUpActionWhenClicked = null;

    }

    public void SetKeyboardCapture(string cap)
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        Debug.Log(cap);
        if (cap == "1")
        {
            WebGLInput.captureAllKeyboardInput = true;
        }
        else
        {
            WebGLInput.captureAllKeyboardInput = false;
        }
#endif
    }

    public void SetGridVisibilty(bool gridVisible)
    {
        creatorControlerScript.grid.SetActive(gridVisible);
    }

    //SPAWNING AND DELETING OBJECTS:
    public void SpawnPrefab(string type)
    {
        Debug.Log(type);
        CreatorSessionManager.SpawnItemInCurrentRoom(type, "pre-defined");
        Debug.Log($"Spawning {type}!");
    }


    public void SpawnGltf(string nameOnServer)
    {
        Debug.Log(nameOnServer);
        CreatorSessionManager.SpawnItemInCurrentRoom(nameOnServer, "user-defined");
    }

    public void DeleteSelectedItem()
    {
        if (creatorControlerScript.selectedObject == null) {return;}
        ShowPopUp("Delete Item","Do you really want to delete the selected Item?","Delete Item", (int i) =>
        {
            if (i == 1)
            {
                GameObject objectToDelete = creatorControlerScript.selectedObject;
                CreatorSessionManager.RemoveItemFromCurrentRoom(objectToDelete.GetComponent<CreatorItem>());
                creatorControlerScript.selectedObject = null;
                creatorControlerScript.UpdateGizmoPosition();
                
            }
        },"Cancel");
        
    }


    //gets the deletedItemName from React 
    public void CustomItemDeletedFromServer(string itemName)
    {
        ShowPopUp("Delete Item","Do you really want to delete the selected Item? It will be removed from the Scene!","Delete Item", (int i) =>
        {
            if (i == 1)
            {
                GameObject room = CreatorSessionManager.GetCurrentRoomGameObject();
                foreach (CreatorItem item in room.GetComponentsInChildren<CreatorItem>())
                {
                    if (item.name == itemName + "-ROOT")
                    {
                        CreatorSessionManager.RemoveItemFromCurrentRoom(item);
                    }
                }
            }
        },"Cancel");
    }

    //Modifying Objects:

    public void ReportObjectInfo()
    {
        GameObject selectedObject = creatorControlerScript.selectedObject;
        if (selectedObject == null)
        {
        #if UNITY_WEBGL == true
            WebGLConnection wgl1 = GetComponent<WebGLConnection>();
            wgl1.ItemInfoToWebGL("",0,new float[]{0});
        #endif
        #if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        //ui.setSelectedObject(null);
        #endif
            return;
        }   
        //Create List off all float parameters
        var position = selectedObject.transform.position;
        var rotation = selectedObject.transform.rotation; 
        var localScale = selectedObject.transform.localScale;
        
        float[] dataList = {position[0],position[1],position[2],rotation.eulerAngles[0],rotation.eulerAngles[1],rotation.eulerAngles[2],localScale[0],localScale[1],localScale[2]};

        #if UNITY_WEBGL == true
        WebGLConnection wgl = GetComponent<WebGLConnection>();
        wgl.ItemInfoToWebGL(selectedObject.name,999,dataList);
        #endif
        #if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        //ui.setSelectedObject(selectedObject);
        #endif
    }

    public void MoveSelectedObjectX(float location)
    {
        GameObject objectToMove = creatorControlerScript.selectedObject;

        Vector3 oldPosition = objectToMove.transform.position;
        objectToMove.transform.position = new Vector3(location, oldPosition[1], oldPosition[2]);

        ReportObjectInfo();
    }

    public void MoveSelectedObjectY(float location)
    {
        GameObject objectToMove = creatorControlerScript.selectedObject;

        Vector3 oldPosition = objectToMove.transform.position;
        objectToMove.transform.position = new Vector3(oldPosition[0], location, oldPosition[2]);
        ReportObjectInfo();
    }

    public void MoveSelectedObjectZ(float location)
    {
        GameObject objectToMove = creatorControlerScript.selectedObject;

        Vector3 oldPosition = objectToMove.transform.position;
        objectToMove.transform.position = new Vector3(oldPosition[0], oldPosition[1], location);
        ReportObjectInfo();
    }
    
    

    public void RotateSelectedObjectX(float rotation)
    {
        GameObject objectToRotate = creatorControlerScript.selectedObject;

        Vector3 oldRotationEulerAngles = objectToRotate.transform.rotation.eulerAngles;
        objectToRotate.transform.transform.rotation = Quaternion.Euler(rotation,oldRotationEulerAngles[1],oldRotationEulerAngles[2]);
        ReportObjectInfo();
    }

    public void RotateSelectedObjectY(float rotation)
    {
        GameObject objectToRotate = creatorControlerScript.selectedObject;

        Vector3 oldRotationEulerAngles = objectToRotate.transform.rotation.eulerAngles;
        objectToRotate.transform.transform.rotation = Quaternion.Euler(oldRotationEulerAngles[0 ],rotation, oldRotationEulerAngles[2] );
        ReportObjectInfo();
    }

    public void RotateSelectedObjectZ(float rotation)
    {
        GameObject objectToRotate = creatorControlerScript.selectedObject;

        Vector3 oldRotationEulerAngles = objectToRotate.transform.rotation.eulerAngles;
        objectToRotate.transform.transform.rotation = Quaternion.Euler(oldRotationEulerAngles[0 ],oldRotationEulerAngles[1], rotation);
        ReportObjectInfo();
    }

    
    
    public void ScaleSelectedObjectX(float scale)
    {
        GameObject objectToScale = creatorControlerScript.selectedObject;

        Vector3 oldScale = objectToScale.transform.localScale;
        objectToScale.transform.localScale = new Vector3(scale, oldScale[1], oldScale[2]);
        ReportObjectInfo();
    }
    
    public void ScaleSelectedObjectY(float scale)
    {
        GameObject objectToScale = creatorControlerScript.selectedObject;

        Vector3 oldScale = objectToScale.transform.localScale;
        objectToScale.transform.localScale = new Vector3(oldScale[0], scale, oldScale[2]);
        ReportObjectInfo();
    }

    public void ScaleSelectedObjectZ(float scale)
    {
        GameObject objectToScale = creatorControlerScript.selectedObject;

        Vector3 oldScale = objectToScale.transform.localScale;
        objectToScale.transform.localScale = new Vector3(oldScale[0], oldScale[1], scale);
        ReportObjectInfo();
    }


        
   

    //GUID FUN:
    /*
     * This function requires the persistenceManager script
     */
    public void SetGUID(string guid)
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.setGUID(guid);
    }

    //LOADING + SAVING ROOMS:
    public void loadRoom(string guid)
    {
        SetGUID(guid);
        CreatorNetworkUtility.LoadEventFromAddress("https://xrchitecture.de/upload/" + guid + "/EventLayout.json", guid);
        Debug.Log("Unity loading room: " + guid);

        // Report back guid
        ReportRoomID();
    }

    public void SaveRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        string guid = pm.getGUID();

        CreatorNetworkUtility.SaveCurrentEventToAddress(guid);
        //ShowReactPopup("Save successful");
    }

    public void newRoom()
    {
        persistenceManager pm = this.GetComponent<persistenceManager>();
        pm.createGUID();
        CreatorSessionManager.CreateNewCreatorEvent(pm.getGUID(),defaultItemsList);
        ReportRoomID();
    }

    public void SaveRoomLocal(string path)
    {
        XrEventContainer eventContainer = CreatorSessionManager.GetCreatorEvent();
        string jsonToUpload = XrJsonUtility.ParseJsonFromEvent(eventContainer);
        File.WriteAllText(path, jsonToUpload);
    }

    public void LoadRoomLocal(string path)
    {
        using StreamReader r = new StreamReader(path);
        string json = r.ReadToEnd();
        XrEventContainer eventContainer = XrJsonUtility.ParseEventFromJson(json);
        CreatorSessionManager.SetCreatorEvent(eventContainer);
    }
    
    //World Modifying:

    public void getSkyBoxList()
    {
        
        #if UNITY_WEBGL == true
        List<string> skyBoxNames = new List<string>();
        foreach (var g in SkyBoxList)
        {
            skyBoxNames.Add(g.name);
        }
        WebGLConnection wgl = GetComponent<WebGLConnection>();
        wgl.SendSkyboxList(skyBoxNames);
#endif


#if UNITY_STANDALONE == true
        PcUiController ui = GetComponent<PcUiController>();
        //ui.setSelectedObject(null);
#endif
    }

    public void setSkybox(string name)
    {
        foreach (var skyboxMaterial in SkyBoxList)
        {
            if (skyboxMaterial.name != name) continue;
            
            RenderSettings.skybox = skyboxMaterial;
            break;
        }
    }
}