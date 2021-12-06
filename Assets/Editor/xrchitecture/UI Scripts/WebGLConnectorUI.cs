using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WebGLConnection))]
public class WebGLConnectorUI : Editor
{
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
        string pathTOGLTF = GUILayout.TextField("test");
        GUILayout.EndHorizontal();
        if (GUILayout.Button("GLTFImport"))
        {
            myTarget.SpawnGltf(pathTOGLTF);
        }
        
        GUILayout.Space(20);
        GUILayout.Label("Persistent Manager Communication");
        
        
        persistenceManager pm = myTarget.GetComponent<persistenceManager>();
        string GUIDTextField = GUILayout.TextField(pm.getGUID());
        
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Create new Room"))
        {
            myTarget.newRoom();
        }
        if(GUILayout.Button("Load a Room"))
        {
            myTarget.loadRoom(GUILayout.TextField(GUIDTextField));
        }
        GUILayout.EndHorizontal();



    }
}
