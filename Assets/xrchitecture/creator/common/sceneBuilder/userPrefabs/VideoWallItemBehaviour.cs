using System.Collections.Generic;
using UnityEngine;



namespace Xrchitecture.Creator.Common.Data
{
    class VideoWallItemBehaviour : ACustomItemBehaviour
    {
        public string url;
        public float volume;
                
        public override void Initialize(List<ItemCustomArgs> args)
        {
            CustomArgsList = args;
            
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

#if UNITY_EDITOR == false && UNITY_WEBGL == false
            //videowall add the Video Component:
            GameObject videoscreen = GetComponentInChildren<MeshRenderer>().gameObject;
            var vid = videoscreen.gameObject.AddComponent<RenderHeads.Media.AVProVideo.MediaPlayer>();
            var texture = videoscreen.gameObject.AddComponent<RenderHeads.Media.AVProVideo.ApplyToMaterial>();

            texture.Material = videoscreen.gameObject.GetComponent<MeshRenderer>().material;
            texture.Player = vid;
            texture.TexturePropertyName = "_BaseMap";
            vid.MediaSource = RenderHeads.Media.AVProVideo.MediaSource.Path;
            vid.MediaPath.PathType = RenderHeads.Media.AVProVideo.MediaPathType.AbsolutePathOrURL;
            vid.MediaPath.Path = url;

            vid.AudioVolume = volume;
            
            vid.Play();
#endif
        }
        
        public override void UpdateCustomArgs(string itemName, string key, string value)
        {
            
            switch (key)
            {
                case "url":
                    url = value;
                    break;
                case "volume":
                    volume = float.Parse(value);
                    break;
            }
            CustomArgsList = new List<ItemCustomArgs>()
            {
                new ItemCustomArgs("url", url),
                new ItemCustomArgs("volume", volume.ToString())
            };
            GetComponentInParent<CreatorItem>().ItemContainer.ItemCustomArgs = CustomArgsList;
        }
    }
}

