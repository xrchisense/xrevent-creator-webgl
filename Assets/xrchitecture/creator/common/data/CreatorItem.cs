using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class CreatorItem : MonoBehaviour
    {
        public ItemContainer ItemContainer { get; private set; }

        public void Initialize(ItemContainer itemContainer)
        {
            ItemContainer = itemContainer;
            
            transform.SetPositionAndRotation(itemContainer.Position, itemContainer.Rotation);
            
            if (itemContainer.ItemCustomArgs != null)
            {
                GetComponentInChildren<ACustomItemBehaviour>().Initialize(itemContainer.ItemCustomArgs);
            }
        }
    }
}