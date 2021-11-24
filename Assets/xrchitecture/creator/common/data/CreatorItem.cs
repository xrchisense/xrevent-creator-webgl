
using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class CreatorItem : MonoBehaviour
    {
        public void Initialize(List<ItemCustomArgs> args = null)
        {
            ACustomItemBehaviour customItemBehaviour = GetComponentInChildren<ACustomItemBehaviour>();

            if (customItemBehaviour != null)
            {
                customItemBehaviour.Initialize(args);
            }
        }
    }
}