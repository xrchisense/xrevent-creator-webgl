using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Xrchitecture.Creator.Common.Data
{
    class VideoWallItemBehaviour : ACustomItemBehaviour
    {
        public string url;
        public float volume;
                
        public override void Initialize(List<ItemCustomArgs> args)
        {

            foreach (var cs in args)
            {
                switch (cs.Argument)
                {
                    case "url":
                        url = cs.Value;
                        break;
                    case "volume":
                        volume = float.Parse(cs.Value);
                        break;
                }
            }
            Debug.LogWarning("init VideoWall");

#if UNITY_EDITOR == false || UNITY_WEBGL == false
            //videowall add the Video Component:
            GameObject videoscreen = GetComponentInChildren<MeshRenderer>().gameObject;
            
                
            vid.url = "https://www.youtube.com/watch?v=JsyyZ9DR_Us";
            vid.Play();
#endif
        }
    }
}

