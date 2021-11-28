// ------------------------------------------------------------------------------------------------------
// <copyright file="XrJsonUtility.cs" company="Monstrum">
// Copyright (c) 2021 Monstrum. All rights reserved.
// </copyright>
// <author>
//   Nikolai Reinke
// </author>
// ------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    public static class XrJsonUtility
    {
        internal static XrEventContainer ParseEventFromJson(string roomString)
        {
            Debug.Log(roomString);
            XrEventContainer xrEventContainer = JsonConvert.DeserializeObject<XrEventContainer>(roomString);
            return xrEventContainer;
        }
    }
}