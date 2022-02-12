/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Xrchitecture.Creator.Common.Data
{
    [CustomEditor(typeof(ItemCustomArgAdd))]
    public class ItemCustomArgAddUI : Editor
    {
        private int i;
        private List<string> valueList = new List<string>();
        private string newKey = "null";
        
        

        public override void OnInspectorGUI()
        {
            
            
            GUILayout.Label("start");
            ItemCustomArgAdd myTarget = (ItemCustomArgAdd) target;
            if (myTarget.CustomArgs == null)
            {
                myTarget.CustomArgs = new List<ItemCustomArgs>()
                {
                    new ItemCustomArgs("cute", "naice")
                };
                /*SetSerializedArray(serializedObject,() => myTarget.CustomArgsList,new List<ItemCustomArgs>().ToArray());#1#
            }
            i = myTarget.CustomArgs.Count;
            
            Debug.Log(myTarget.CustomArgs.Count);
            i = myTarget.CustomArgs.Count;
            for (int j = 0; j < i; j++)
            {
                valueList.Add(myTarget.CustomArgs[j].Value);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Key:",GUILayout.ExpandWidth(false));
                GUILayout.Label(myTarget.CustomArgs[j].Argument);
                GUILayout.Label("value:",GUILayout.ExpandWidth(false));
                GUILayout.Label(myTarget.CustomArgs[j].Value);
                valueList[j] = GUILayout.TextField(valueList[j]);
                if (GUILayout.Button("update Argument"))
                {
                    myTarget.CustomArgs[j].Value = valueList[j];
                    /*SetSerializedArray<ItemCustomArgs>(serializedObject,() => myTarget.CustomArgsList,valueList.ToArray());#1#
                }
                if (GUILayout.Button("delete Argument"))
                {
                    myTarget.CustomArgs.RemoveAt(j);
                    valueList.RemoveAt(j);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.BeginHorizontal();
            newKey = GUILayout.TextField(newKey);
            if (GUILayout.Button("add Argument0"))
            {
                myTarget.CustomArgs.Add(new ItemCustomArgs(newKey,"null"));
                valueList.Add("null");
            }
            GUILayout.EndHorizontal();
            GUILayout.Label("end");
            
            
        }
        
        
        public static void SetSerializedArray<T>(SerializedObject serializedObject, Expression<Func<T>> memberAccess, Array newArray) {
            string fieldName = ((MemberExpression)memberAccess.Body).Member.Name;
            SerializedProperty property = serializedObject.FindProperty(fieldName);
 
            property.arraySize = newArray.Length;
 
            for (int i = 0; i < newArray.Length; i++) {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
 
                switch (element.propertyType) {
                    case SerializedPropertyType.Integer:
                        element.intValue = (int)newArray.GetValue(i);
                        break;
                    case SerializedPropertyType.Boolean:
                        element.boolValue = (bool)newArray.GetValue(i);
                        break;
                    case SerializedPropertyType.Float:
                        element.floatValue = (float)newArray.GetValue(i);
                        break;
                    case SerializedPropertyType.String:
                        element.stringValue = (string)newArray.GetValue(i);
                        break;
                    case SerializedPropertyType.Color:
                        element.colorValue = (Color)newArray.GetValue(i);
                        break;
                    case SerializedPropertyType.ObjectReference:
                        element.objectReferenceValue = (UnityEngine.Object)newArray.GetValue(i);
                        break;
                    default:
                        Debug.LogError("SetSerializedArray unhandled array type " + element.propertyType);
                        break;
                }
            }
 
            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif*/