using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class XrEventCustomArgs
    {
        [JsonProperty("argument")]
        public string Argument;
        [JsonProperty("value")]
        public string Value;
    }
}