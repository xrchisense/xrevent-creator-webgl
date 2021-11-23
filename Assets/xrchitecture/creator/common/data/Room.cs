using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class Room
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("jsonversion")]
        public float JsonVersion;
        [JsonProperty("items")]
        public List<Item> Items;

        public override string ToString()
        {
            return ($"Name: " + Name + ", Version: " + JsonVersion);
        }
    }
}