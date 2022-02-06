using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

public class EditorPopUp : EditorWindow
{
    private static string Titel = "Defaul Titel";
    private static string Body = "Default Body";
    private static string Button1 = "Default";
    private static string Button2 = "null";
    private static string Button3 = "null";
    
    
    [MenuItem("Window/My Window")]
    static void Init()
    {
        HelperBehaviour.Instance.LevelManager.OnBringUpWindow += ShowEditorPopUp;
        Debug.Log("windows hooked!");
    }
    
    static void ShowEditorPopUp(string titelString, string bodyTextString, string button1Text, string button2Text, string button3Text, bool showX)
    {
        Titel = titelString;
        Body = bodyTextString;
        Button1 = button1Text;
        Button2 = button2Text;
        Button3 = button3Text;

        // Get existing open window or if none, make a new one:
        EditorPopUp window = (EditorPopUp) EditorWindow.GetWindow(typeof(EditorPopUp));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label(Titel, EditorStyles.boldLabel);
        GUILayout.TextArea(Body);
        GUILayout.BeginVertical();
        if (GUILayout.Button(Button1))
        {
            HelperBehaviour.Instance.LevelManager.PopUpFeedback(1);
            
        }

        if (Button2 != "null")
        {
            if (GUILayout.Button(Button2))
            {
                HelperBehaviour.Instance.LevelManager.PopUpFeedback(2);
            }
        }

        if (Button3 != "null")
        {
            if (GUILayout.Button(Button3))
            {
                HelperBehaviour.Instance.LevelManager.PopUpFeedback(3);
            }
        }

        GUILayout.EndVertical();
    }
}