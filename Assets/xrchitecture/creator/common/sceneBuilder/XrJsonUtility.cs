using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    public static class XrJsonUtility
    {
        internal static XrEventContainer ParseEventFromJson(string roomString)
        {
            XrEventContainer xrEventContainer =
                JsonConvert.DeserializeObject<XrEventContainer>(roomString);
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