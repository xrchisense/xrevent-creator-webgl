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
        public List<ItemCustomPar> ItemCustomArgs;

        
        // Maybe use a dictionary instead
        public void UpdateCustomPar(ItemCustomPar newCustomPar)
        {
            foreach (ItemCustomPar itemCustomArg in ItemCustomArgs)
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