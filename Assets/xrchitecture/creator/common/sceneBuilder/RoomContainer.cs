using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class RoomContainer
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("items")]
        public List<ItemContainer> Items;
        [JsonProperty("guid")]
        public string Guid;
        [JsonProperty("skybox")]
        public string Skybox;
        [JsonProperty("room-custom-args")]
        public List<RoomCustomArgs> RoomCustomArgs;
    }
}