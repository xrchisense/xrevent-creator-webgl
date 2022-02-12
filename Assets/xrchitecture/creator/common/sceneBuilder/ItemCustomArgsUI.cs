using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR

namespace Xrchitecture.Creator.Common.Data
{
    [CustomEditor(typeof(ItemCustomArgs))]
    public class ItemCustomArgsUI : Editor
    {
        private string _value;
        private string _key;
        public override void OnInspectorGUI()
        {
            _value = GUILayout.TextField(serializedObject.FindProperty("value").ToString());
        }
    }
}

#endif