using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Xrchitecture.Creator.Common.Data
{
    public static class XrJsonVersionRepair
    {
        internal static XrEventContainer UpdateEventContainer(XrEventContainer oldContainer)
        {
            //one Part for every Update to the JSON:
            
            //0.2 Introduced Scale!
            if (oldContainer.JsonVersion < 0.2)
            {
                foreach (var room in oldContainer.Rooms)
                {
                    foreach (var item in room.Items)
                    {
                        item.Scale = new Vector3(1, 1, 1);
                    }
                }
            }
            //0.3 SkyBox Save
            if (oldContainer.JsonVersion < 0.3)
            {
                foreach (var room in oldContainer.Rooms)
                {
                    room.Skybox = "Day_BlueSky_Nothing";
                }
            }
            
            //0.X XXXXXXXXXX
            /*if (oldContainer.JsonVersion < 0.9)
            {
                
            }*/



            oldContainer.JsonVersion = HelperBehaviour.Instance.currentJsonVersion;
            return oldContainer;
        }
    }
}