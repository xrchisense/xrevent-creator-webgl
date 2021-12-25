using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
namespace Xrchitecture.Creator.Common.Data
{
    [CustomEditor(typeof(CreatorItem))]
    internal class CreatorItemUI : Editor
    {

        public override void OnInspectorGUI()
        {
            CreatorItem myTarget = (CreatorItem) target;

            GUILayout.Label(myTarget.ItemContainer.ResourceName);
        }

    }
}

#endif

