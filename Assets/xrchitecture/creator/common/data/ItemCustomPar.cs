using System;
using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal class ItemCustomPar : IComparable<ItemCustomPar>
    {
        [JsonConstructor]
        internal ItemCustomPar(string argument, string value)
        {
            Argument = argument;
            Value = value;
        }

        [JsonProperty("argument")]
        public string Argument { get; private set; }

        [JsonProperty("value")]
        public string Value { get; private set; }


        public int CompareTo(ItemCustomPar other)
        {
            return string.Compare(Argument, other.Argument);
        }
    }
}