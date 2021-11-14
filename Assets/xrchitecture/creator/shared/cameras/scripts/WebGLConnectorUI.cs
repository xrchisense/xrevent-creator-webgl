#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WegGLConnection))]
public class WebGLConnectorUI:Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        WegGLConnection myTarget = (WegGLConnection) target;
        
        if(GUILayout.Button("Spawn Sphere"))
        {
            myTarget.SpawnItemEvent("Sphere");
        }
        if(GUILayout.Button("Spawn Light"))
        {
            myTarget.SpawnItemEvent("Lamp");
        }
    }
}
#endif