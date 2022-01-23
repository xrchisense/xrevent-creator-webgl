using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class CreatorItem : MonoBehaviour
    {
        public ItemContainer ItemContainer { get; private set; }

        private ACustomItemBehaviour _aCustomItemBehaviour;

        public void Initialize(ItemContainer itemContainer)
        {
            ItemContainer = itemContainer;
            
            transform.SetPositionAndRotation(itemContainer.Position, itemContainer.Rotation);
            transform.localScale = itemContainer.Scale;
            
            if (itemContainer.ItemCustomArgs != null)
            {
                _aCustomItemBehaviour = GetComponentInChildren<ACustomItemBehaviour>();
                
                _aCustomItemBehaviour.Initialize(itemContainer.ItemCustomArgs);
                _aCustomItemBehaviour.OnCustomParameterChanged += HandleCustomParameterChanged;
            }
        }

        

        public GameObject GetThisObjectRoot()
        {
            return gameObject;
        }
        
        //When Object is deleted:
        private void OnDisable()
        {
            CreatorSessionManager.RemoveItemFromCurrentRoom(this);
        }

        /// <summary>
        /// Function to be called manually, after the GameObject has been moved via Gizmo/Code/etc.
        /// </summary>
        public void TransformUpdated()
        {
            ItemContainer.Position = transform.position;
            ItemContainer.Rotation = transform.rotation;
            ItemContainer.Scale = transform.localScale;
        }

        /// <summary>
        /// Handles the event from a ACustomItemBehaviour when a ItemCustomArgument has changed
        /// </summary>
        /// <param name="itemCustomPar">The ItemCustomPar that has changed</param>
        private void HandleCustomParameterChanged(ItemCustomArgs itemCustomPar)
        {
            ItemContainer.UpdateCustomPar(itemCustomPar);
        }

        private void OnDestroy()
        {
            if (_aCustomItemBehaviour)
            {
                _aCustomItemBehaviour.OnCustomParameterChanged -= HandleCustomParameterChanged;
            }
        }
    }
}