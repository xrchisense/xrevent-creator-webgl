using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    class PointLightItemBehaviour : ACustomItemBehaviour
    {
        public Color color = Color.red;
        public float brightness = 1f;
        
        public override void Initialize(List<ItemCustomArgs> args)
        {
            CustomArgsList = args;

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

        public override void UpdateCustomArgs(string itemName, string key, string value)
        {
            Light lamp = GetComponentInChildren<Light>();
            switch (key)
            {
                case "color":
                    if (ColorUtility.TryParseHtmlString(value, out color)) { lamp.color = color;}
                    break;
                case "brightness":
                    brightness = float.Parse(value);
                    lamp.intensity = brightness;
                    break;
            }
            
            
            

            CustomArgsList = new List<ItemCustomArgs>()
            {
                new ItemCustomArgs("color", "#" + ColorUtility.ToHtmlStringRGB(color)),
                new ItemCustomArgs("brightness", brightness.ToString())
            };
            GetComponentInParent<CreatorItem>().ItemContainer.ItemCustomArgs = CustomArgsList;
        }
    }
}