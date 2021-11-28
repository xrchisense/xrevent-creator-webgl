using System;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemCustomArg : IComparable<ItemCustomArg>
    {
        [JsonConstructor]
        internal ItemCustomArg(string argument, string value)
        {
            Argument = argument;
            Value = value;
        }

        [JsonProperty("argument")]
        public string Argument { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }


        public int CompareTo(ItemCustomArg other)
        {
            return string.Compare(Argument, other.Argument);
        }
    }
}