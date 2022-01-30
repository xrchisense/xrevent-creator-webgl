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
    
    [DllImport("__Internal")]
    private static extern void ReportLoadingStatus(int percent);
    
    [DllImport("__Internal")]
    private static extern void SkyboxList(string  skyboxList);

    public void ItemInfoToWebGL(string itemName, int itemID, float[] datalist)
    {
        
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ItemInfo (itemName, itemID, datalist, datalist.Length);
        Debug.Log("Untiy did send ItemInfo");
        return;
#endif
    }

    public void ReportLoadingStatusToWebGL(int percent)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ReportLoadingStatus(percent);
        Debug.Log("Unity did send ReportLoadingStatus: " + percent);
        return;
#endif
    }
    public void ShowReactPopup(string titelString,string bodyTextString,string button1Text,string button2Text,string button3Text,bool showX)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ShowPopup (titelString,bodyTextString,button1Text,button2Text,button3Text,showX);
        Debug.Log("Unity did send ShowPopup");
        return;
#endif
    }

    public void ReportRoomIdUnity(string guid)
    {
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        ReportRoomID (guid);
        Debug.Log("Unity did send RoomID to Webgl");
        return;
#endif
    }

    public void SendSkyboxList(List<string> skyboxlist)
    {
        string skyboxListLong = "default";
        foreach (var name in skyboxlist)
        {
            skyboxListLong = skyboxListLong + "|" + name;
        }
#if UNITY_WEBGL == true && UNITY_EDITOR == false
        SkyboxList(skyboxListLong);
        Debug.Log("Unity did send SkyboxList to Webgl");
        return;
#endif
    }
}