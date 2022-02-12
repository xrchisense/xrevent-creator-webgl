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
            
            //if item is fresh spawned
            if (itemContainer.ItemCustomArgs == null && GetComponentInChildren(typeof(ItemCustomArgAdd)))
            {
                ItemCustomArgAdd t = GetComponentInChildren<ItemCustomArgAdd>();
                itemContainer.ItemCustomArgs = t.GETCustomArgs();
                Destroy(t);
            }
            
            //if item already has customArgs or just got them
            if (itemContainer.ItemCustomArgs != null)
            {
                _aCustomItemBehaviour = GetComponentInChildren<ACustomItemBehaviour>();
                _aCustomItemBehaviour.Initialize(itemContainer.ItemCustomArgs);
                /*_aCustomItemBehaviour.OnCustomParameterChanged += HandleCustomParameterChanged;*/
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

        public void CustomParametersUpdated()
        {
            if (ItemContainer.ItemCustomArgs == null) return;
            ItemContainer.ItemCustomArgs = GetComponentInChildren<ACustomItemBehaviour>().CustomArgsList;
        }

        /*/// <summary>
        /// Handles the event from a ACustomItemBehaviour when a ItemCustomArgument has changed
        /// </summary>
        /// <param name="itemCustomPar">The ItemCustomPar that has changed</param>
        private void HandleCustomParameterChanged(ItemCustomArgs itemCustomPar)
        {
            ItemContainer.UpdateCustomPar(itemCustomPar);
        }*/

        private void OnDestroy()
        {
            if (_aCustomItemBehaviour)
            {
                /*_aCustomItemBehaviour.OnCustomParameterChanged -= HandleCustomParameterChanged;*/
            }
        }
    }
}