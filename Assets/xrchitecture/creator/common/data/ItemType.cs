// ------------------------------------------------------------------------------------------------------
// <copyright file="ItemType.cs" company="Monstrum">
// Copyright (c) 2021 Monstrum. All rights reserved.
// </copyright>
// <author>
//   Nikolai Reinke
// </author>
// ------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace Xrchitecture.Creator.Common.Data
{
    internal enum ItemType
    {
        [JsonProperty("unknown")]
        Unknown = -1,
        [JsonProperty("none")]
        None = 0,
        [JsonProperty("pre-defined")]
        PreDefined = 1,
        [JsonProperty("user-defined")]
        UserDefined = 2,
    }
}