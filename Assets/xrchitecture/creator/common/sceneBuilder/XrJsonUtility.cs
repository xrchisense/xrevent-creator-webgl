using Newtonsoft.Json;
using System;
using UnityEngine;


namespace Xrchitecture.Creator.Common.Data
{
    public static class XrJsonUtility
    {
        internal static XrEventContainer ParseEventFromJson(string roomString)
        {
            XrEventContainer xrEventContainer = new XrEventContainer();
            try
            {
               xrEventContainer = JsonConvert.DeserializeObject<XrEventContainer>(roomString);
            }
            catch (Exception e)
            {
                Debug.Log("Error parsing the File, it might be damanged. Error Message Below: ");
                Debug.Log(e);
            }
            return xrEventContainer;
        }

        internal static string ParseJsonFromEvent(XrEventContainer xrEventContainer)
        {
            string eventJson = JsonConvert.SerializeObject(xrEventContainer,
                new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });
            return eventJson;
        }
    }
}