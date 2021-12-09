using System;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemCustomArgs : IComparable<ItemCustomArgs>
    {
        [JsonProperty("argument")]
        public string Argument { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }


        [JsonConstructor]
        internal ItemCustomArgs(string argument, string value)
        {
            Argument = argument;
            Value = value;
        }

        public int CompareTo(ItemCustomArgs other)
        {
            return string.Compare(Argument, other.Argument);
        }
    }
}