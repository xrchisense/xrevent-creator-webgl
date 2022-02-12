using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    class PointLightItemBehaviour : ACustomItemBehaviour
    {
        
        
        public override void Initialize(List<ItemCustomArgs> args)
        {
            Color color = Color.red;
            float brightness = 1f;

            foreach (var cs in args)
            {
                switch (cs.Argument)
                {
                    case "color":
                        if (ColorUtility.TryParseHtmlString(cs.Value, out color)) { }
                        break;
                    case "brightness":
                        brightness = float.Parse(cs.Value);
                        break;
                }
            }

            Light lamp = GetComponentInChildren<Light>();

            lamp.color = color;
            lamp.intensity = brightness;
            
            Debug.LogWarning("init PointLight");
        }
    }
}