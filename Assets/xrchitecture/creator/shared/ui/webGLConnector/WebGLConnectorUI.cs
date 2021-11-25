#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WebGLConnection))]
public class WebGLConnectorUI:Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WebGLConnection myTarget = (WebGLConnection) target;

        foreach (var x in myTarget.PrefabList)
        {
            if(GUILayout.Button($"Spawn {x.name}"))
            {
                myTarget.SpawnPrefab(x.name);
            }
        }
    }
}
#endif