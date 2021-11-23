
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemCustomArgs
    {
        [JsonProperty("argument")]
        string Argument;
        [JsonProperty("value")]
        string Value;
    }
}

