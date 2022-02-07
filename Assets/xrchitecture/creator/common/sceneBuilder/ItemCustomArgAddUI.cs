using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Xrchitecture.Creator.Common.Data
{
    [CustomEditor(typeof(ItemCustomArgAdd))]
    public class ItemCustomArgAddUI : Editor
    {
        private int i = 1;
        
        public override void OnInspectorGUI()
        {
            GUILayout.Label("start");
            ItemCustomArgAdd myTarget = (ItemCustomArgAdd) target;
            /*i = int.Parse(GUILayout.TextField(i.ToString()));*/
            /*for (int j = 0; j < i; j++)
            {
                try
                {
                    string test = myTarget.CustomArgsList[i].Argument;
                }
                catch (Exception e)
                {
                    myTarget.CustomArgsList.Add(new ItemCustomArgs("",""));
                }
            }*/
            Debug.Log(myTarget.CustomArgsList.Count);
            i = myTarget.CustomArgsList.Count;
            for (int j = 0; j < i; j++)
            {
                GUILayout.BeginVertical();
                GUILayout.TextField(myTarget.CustomArgsList[j].Argument);
                GUILayout.TextField(myTarget.CustomArgsList[j].Value);
                GUILayout.BeginVertical();
            }
            GUILayout.Label("end");
            
        }
    }
}
#endif