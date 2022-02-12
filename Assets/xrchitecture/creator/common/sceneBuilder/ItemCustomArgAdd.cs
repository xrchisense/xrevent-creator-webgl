using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    
    //this needs to be added to an prefab and will be scanned for when item is spawned
    [System.Serializable]
    internal class ItemCustomArgAdd : MonoBehaviour
    {
        [SerializeField]
        public List<ItemCustomArgs> CustomArgsList = new List<ItemCustomArgs>()
        {
            new ItemCustomArgs("null","null"),
        };
    }
}