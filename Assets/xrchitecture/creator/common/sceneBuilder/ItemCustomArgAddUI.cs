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
        private List<string> valueList = new List<string>();
        private string newKey = "null";

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
                valueList.Add(myTarget.CustomArgsList[j].Value);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Key:",GUILayout.ExpandWidth(false));
                GUILayout.Label(myTarget.CustomArgsList[j].Argument);
                GUILayout.Label("value:",GUILayout.ExpandWidth(false));
                GUILayout.Label(myTarget.CustomArgsList[j].Value);
                valueList[j] = GUILayout.TextField(valueList[j]);
                if (GUILayout.Button("update Argument"))
                {
                    myTarget.CustomArgsList[j].Value = valueList[j];
                }
                if (GUILayout.Button("delete Argument"))
                {
                    myTarget.CustomArgsList.RemoveAt(j);
                    valueList.RemoveAt(j);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            newKey = GUILayout.TextField(newKey);
            if (GUILayout.Button("add Argument0"))
            {
                myTarget.CustomArgsList.Add(new ItemCustomArgs(newKey,"null"));
                valueList.Add("null");
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("end");
            
        }
    }
}
#endif