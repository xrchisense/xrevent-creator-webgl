using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Xrchitecture.Creator.Common.Data
{
    class VideoWallItemBehaviour : ACustomItemBehaviour
    {
        private List<ItemCustomArgs> _itemCustomArgs = new List<ItemCustomArgs>();
        public string VideoURL;
        
        public override void Initialize(List<ItemCustomArgs> args)
        {
            
            Debug.Log("INIT WVIDOEEWASDJAWED");
            //set the screen to a better position after spawning
            
            transform.position = new Vector3(0, 1, 0);
            transform.localEulerAngles = new Vector3(-90, 0, 0);
           
            
            //videowall add the Video Component:
            GameObject videoscreen = GetComponentInChildren<MeshRenderer>().gameObject;
            VideoPlayer vid = videoscreen.gameObject.AddComponent<VideoPlayer>();
                
            vid.url = "https://www.youtube.com/watch?v=JsyyZ9DR_Us";
            vid.Play();
        }
    }
}

