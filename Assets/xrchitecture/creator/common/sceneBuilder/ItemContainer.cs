using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemContainer
    {
        [JsonProperty("position")]
        public Vector3 Position;
        [JsonProperty("rotation")]
        public Quaternion Rotation;
        [JsonProperty("scale")]
        public Vector3 Scale;
        [JsonProperty("type")]
        public string ItemType;
        [JsonProperty("resourcename")]
        public string ResourceName;
        [JsonProperty("item-custom-args")]
        public List<ItemCustomArgs> ItemCustomArgs;

        
        // Maybe use a dictionary instead
        public void UpdateCustomPar(ItemCustomArgs newCustomPar)
        {
            foreach (ItemCustomArgs itemCustomArg in ItemCustomArgs)
            {
                if (itemCustomArg.Argument == newCustomPar.Argument)
                {
                    ItemCustomArgs.Remove(itemCustomArg);
                    break;
                }
            }
            
            ItemCustomArgs.Add(newCustomPar);
            
            ItemCustomArgs.Sort(); // sorting to avoid some confusion when debugging and iterating over indices while updating ItemCustomArgs
        }
    }


}