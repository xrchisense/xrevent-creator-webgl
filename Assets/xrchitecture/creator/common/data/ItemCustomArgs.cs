
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemCustomArgs
    {
        [JsonProperty("argument")]
        public string Argument;
        [JsonProperty("value")]
        public string Value;
    }
}

