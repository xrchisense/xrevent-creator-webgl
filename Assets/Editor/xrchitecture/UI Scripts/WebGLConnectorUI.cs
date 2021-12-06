using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WebGLConnection))]
public class WebGLConnectorUI : Editor
{
    private bool localRoomFile;
    
    string GUIDTextField = "";
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GUILayout.Space(20);
        GUILayout.Label("Spawn Primitive List:");
        

        WebGLConnection myTarget = (WebGLConnection) target;

        foreach (var x in myTarget.PrefabList)
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
        string pathTOGLTF = GUILayout.TextField("https://xrchitecture.de/upload/0f8fad5b-d9cb-469f-a165-70867728950e/items/sonic-the-hedgehog.gltf");
        GUILayout.EndHorizontal();
        if (GUILayout.Button("GLTFImport"))
        {
            myTarget.SpawnGltf(pathTOGLTF);
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Persistent Manager Communication");
        persistenceManager pm = myTarget.GetComponent<persistenceManager>();
        GUILayout.BeginHorizontal();
        
        if (localRoomFile == false)
        {
            if (GUILayout.Button("switch to local", GUILayout.ExpandWidth(false)))
            {
                localRoomFile = true;
            }
            GUIDTextField = GUILayout.TextField("0f8fad5b-d9cb-469f-a165-70867728950e");
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
                myTarget.roomSaverLoader.LoadRoom(true,GUIDTextField);
                return;   
            }
            myTarget.loadRoom(GUIDTextField);
        }
        if(GUILayout.Button("Save a Room"))
        {
            if (localRoomFile)
            {
                myTarget.roomSaverLoader.SaveRoom(true,GUIDTextField);
                return;   
            }
            myTarget.SaveRoom(GUIDTextField);
        }
        GUILayout.EndHorizontal();



    }
}
