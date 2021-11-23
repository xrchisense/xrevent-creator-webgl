using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal class Item
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
        public List<ItemCustomArgs> ItemCustomArgs;
    }


}