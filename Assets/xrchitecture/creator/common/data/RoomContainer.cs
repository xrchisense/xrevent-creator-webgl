using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class RoomContainer
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("jsonversion")]
        public float JsonVersion;
        [JsonProperty("items")]
        public List<ItemContainer> Items;
        [JsonProperty("room-custom-args")]
        public List<RoomCustomArgs> RoomCustomArgs;

        public override string ToString()
        {
            return ($"Name: " + Name + ", Version: " + JsonVersion);
        }
    }
}