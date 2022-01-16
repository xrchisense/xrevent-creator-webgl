using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WebGLConnection : MonoBehaviour
{
    //
    //
    //   These are the messages sent via jslib plugin to the react app
    //
    // 
    [DllImport("__Internal")]
    private static extern void ItemInfo(string itemName, int itemID, float[] dataarray,int arrayLength);

    [DllImport("__Internal")]
    private static extern void ShowPopup(string titelString,string bodyTextString,string button1Text,string button2Text,string button3Text,bool showX);

    [DllImport("__Internal")]
    private static extern void ReportRoomID(string id);

    public void ItemInfoToWebGL(string itemName, int itemID, float[] datalist)
    {
        
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ItemInfo (itemName, itemID, datalist, datalist.Length);
        Debug.Log("Untiy did send ItemInfo");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }

    public void ShowReactPopup(string titelString,string bodyTextString,string button1Text,string button2Text,string button3Text,bool showX)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ShowPopup (titelString,bodyTextString,button1Text,button2Text,button3Text,showX);
        Debug.Log("Unity did send ShowPopup");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send ItemInfo");
    }

    public void ReportRoomIdUnity(string guid)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ReportRoomID (guid);
        Debug.Log("Unity did send RoomID to Webgl");
        return;
#endif
        Debug.Log("NOT RUNNING IN WEBGL: DID NOT send reportRoomIdUnity");
    }
}