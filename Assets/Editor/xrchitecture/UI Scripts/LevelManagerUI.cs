using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CreatorLevelManager))]
public class LevelManagerUI : UnityEditor.Editor
{
    private bool localRoomFile;
    
    string GUIDTextField = "";
    string newGUIDTextField = "";

    string pathTOGLTF =
        "bag-chair.gltf";

    private string key = "";
    private string value = "";

    private float moveTextField = 0f;
    private float rotateTextField = 0f;
    private float scaleTextField = 0f;
    public override void OnInspectorGUI()
    {
        CreatorLevelManager myTarget = (CreatorLevelManager) target;
        
        
        DrawDefaultInspector();

        TestConfigHelper.PrefabList = myTarget.prefabList;
        
        GUILayout.Space(20);
        if(GUILayout.Button("Delete Selected Object"))
        {
            myTarget.DeleteSelectedItem();
        }
        GUILayout.Space(20);
        GUILayout.Label("Spawn Primitive List:");
        

        

        foreach (var x in TestConfigHelper.PrefabList)
        {
            if(GUILayout.Button($"Spawn {x.name}"))
            {
                myTarget.SpawnPrefab(x.name);
            }
        }
        GUILayout.Space(20);
        GUILayout.Label("GLTF Importer");
        
        GUILayout.BeginHorizontal();
        GUILayout.Label("Path to GLTF:     ",GUILayout.ExpandWidth(false));
        pathTOGLTF = GUILayout.TextField(pathTOGLTF);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("GLTFImport"))
        {
            myTarget.SpawnGltf(pathTOGLTF);
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Persistent Manager Communication");
        persistenceManager pm = myTarget.GetComponent<persistenceManager>();
        GUILayout.BeginHorizontal();
        GUILayout.BeginHorizontal();
        
        newGUIDTextField = GUILayout.TextField(newGUIDTextField);
        if (GUILayout.Button("change GUID", GUILayout.ExpandWidth(false)))
        {
            pm.setGUID(newGUIDTextField);
        }
        GUILayout.EndHorizontal();
        if (localRoomFile == false)
        {
            if (GUILayout.Button("switch to local", GUILayout.ExpandWidth(false)))
            {
                localRoomFile = true;
            }
            GUIDTextField = GUILayout.TextField(pm.getGUID());
        }
        else
        {
            if (GUILayout.Button("switch to Xrchitecture.de webserver", GUILayout.ExpandWidth(false)))
            {
                localRoomFile = false;
            }
            GUIDTextField = GUILayout.TextField(Application.persistentDataPath + "/TestRooms/test.json");
        }
        
        GUILayout.EndHorizontal();
        
        
        
        
        
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create new Room"))
        {
            myTarget.newRoom();
        }
        if(GUILayout.Button("Load a Room"))
        {
            if (localRoomFile)
            {
                Debug.Log(new System.IO.FileInfo(GUIDTextField).Exists);
                myTarget.LoadRoomLocal(GUIDTextField);
                return;   
            }
            myTarget.loadRoom(GUIDTextField);
        }
        if(GUILayout.Button("Save a Room"))
        {
            if (localRoomFile)
            {
                myTarget.SaveRoomLocal(GUIDTextField);
                return;   
            }
            myTarget.SaveRoom();
        }
        GUILayout.EndHorizontal();
        
        GUILayout.BeginHorizontal();
        
        moveTextField = float.Parse(GUILayout.TextField(moveTextField.ToString()));
        if (GUILayout.Button("MoveX", GUILayout.ExpandWidth(false)))
        {
            myTarget.MoveSelectedObjectX(moveTextField);
        }
        if (GUILayout.Button("MoveY", GUILayout.ExpandWidth(false)))
        {
            myTarget.MoveSelectedObjectY(moveTextField);
        }
        if (GUILayout.Button("MoveZ", GUILayout.ExpandWidth(false)))
        {
            myTarget.MoveSelectedObjectZ(moveTextField);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        
        rotateTextField = float.Parse(GUILayout.TextField(rotateTextField.ToString()));
        if (GUILayout.Button("RotateX", GUILayout.ExpandWidth(false)))
        {
            myTarget.RotateSelectedObjectX(rotateTextField);
        }
        if (GUILayout.Button("RotateY", GUILayout.ExpandWidth(false)))
        {
            myTarget.RotateSelectedObjectY(rotateTextField);
        }
        if (GUILayout.Button("RotateZ", GUILayout.ExpandWidth(false)))
        {
            myTarget.RotateSelectedObjectZ(rotateTextField);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        
        scaleTextField = float.Parse(GUILayout.TextField(scaleTextField.ToString()));
        if (GUILayout.Button("ScaleX", GUILayout.ExpandWidth(false)))
        {
            myTarget.ScaleSelectedObjectX(scaleTextField);
        }
        if (GUILayout.Button("ScaleX", GUILayout.ExpandWidth(false)))
        {
            myTarget.ScaleSelectedObjectY(scaleTextField);
        }
        if (GUILayout.Button("ScaleX", GUILayout.ExpandWidth(false)))
        {
            myTarget.ScaleSelectedObjectZ(scaleTextField);
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        key = GUILayout.TextField(key);
        value = GUILayout.TextField(value);
        if (GUILayout.Button("change CustomArg", GUILayout.ExpandWidth(false)))
        {
            myTarget.ChangeCustomArg(key+ "," + value);
        }
        
        GUILayout.EndHorizontal();
        GUILayout.Space(20);
        
        foreach (var x in myTarget.SkyBoxList)
        {
            if(GUILayout.Button($"Spawn {x.name}"))
            {
                myTarget.setSkybox(x.name);
            }
        }

    }
}
