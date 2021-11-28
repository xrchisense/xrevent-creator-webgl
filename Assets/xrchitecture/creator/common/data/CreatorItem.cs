using System;
using System.Collections.Generic;
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
            
            if (itemContainer.ItemCustomArgs != null)
            {
                _aCustomItemBehaviour = GetComponentInChildren<ACustomItemBehaviour>();
                
                _aCustomItemBehaviour.Initialize(itemContainer.ItemCustomArgs);
                _aCustomItemBehaviour.OnCustomParameterChanged += HandleCustomParameterChanged;
            }
        }

        private void HandleCustomParameterChanged(ItemCustomArg itemCustomArg)
        {
            ItemContainer.UpdateCustomArg(itemCustomArg);
        }


        private void OnDestroy()
        {
            if (_aCustomItemBehaviour)
            {
                _aCustomItemBehaviour.OnCustomParameterChanged -=
                    HandleCustomParameterChanged;
            }
        }
    }
}