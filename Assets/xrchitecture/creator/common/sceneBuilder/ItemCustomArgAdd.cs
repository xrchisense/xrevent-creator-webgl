using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    
    //this needs to be added to an prefab and will be scanned for when item is spawned
    
    internal class ItemCustomArgAdd : MonoBehaviour
    {
        /*public List<ItemCustomArgs> CustomArgs;*/

        public List<string> keys;
        public List<string> values;

        
        public List<ItemCustomArgs> GETCustomArgs()
        {
            int i = 0;
            List<ItemCustomArgs> customArgs = new List<ItemCustomArgs>();
            foreach (var customKey in keys)
            {
                customArgs.Add(new ItemCustomArgs(customKey,values[i]));
                i++;
            }

            return customArgs;
        }
    }
}