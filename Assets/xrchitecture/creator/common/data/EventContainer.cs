using System.Collections.Generic;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class XrEventContainer
    {
        [JsonProperty("name")]
        public string Name;
        [JsonProperty("rooms")]
        public RoomContainer[] Rooms;
        [JsonProperty("event-custom-args")]
        public List<XrEventCustomArgs> XrEventCustomArgs;
    }
}

