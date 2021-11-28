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
        [JsonProperty("type")]
        public string ItemType;
        [JsonProperty("resourcename")]
        public string ResourceName;
        [JsonProperty("item-custom-args")]
        public List<ItemCustomArg> ItemCustomArgs;

        
        // Maybe use a dictionary instead
        public void UpdateCustomArg(ItemCustomArg newCustomArg)
        {
            foreach (ItemCustomArg itemCustomArg in ItemCustomArgs)
            {
                if (itemCustomArg.Argument == newCustomArg.Argument)
                {
                    ItemCustomArgs.Remove(itemCustomArg);
                    break;
                }
            }
            
            ItemCustomArgs.Add(newCustomArg);
            
            ItemCustomArgs.Sort(); // sorting to avoid some confusion when debugging and iterating over indices while updating ItemCustomArgs
        }
    }


}