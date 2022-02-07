using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    
    //this needs to be added to an prefab and will be scanned for when item is spawned
    [System.Serializable]
    internal class ItemCustomArgAdd : MonoBehaviour
    {
        public List<ItemCustomArgs> CustomArgsList = new List<ItemCustomArgs>()
        {
            new ItemCustomArgs("test","test"),
            new ItemCustomArgs("test2","test2")
        };
        public List<string> key;
        public List<string> value;
        
        
    }
}