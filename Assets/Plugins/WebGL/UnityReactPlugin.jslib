mergeInto(LibraryManager.library, {
  ItemInfo: function (itemName, itemID, dataarray,arraySize) {
    
    var itemdata = [];
    for (var i = 0; i < arraySize; i++){
      itemdata.push(HEAPF32[(dataarray >> 2) + i]);
    }

    window.dispatchReactUnityEvent(
      "ItemInfo",
      Pointer_stringify(itemName),
      itemID,
      itemdata
    );
  },
  ShowPopup: function (TitelString,BodyTextString,Button1Text,Button2Text,Button3Text,showX) {
    window.dispatchReactUnityEvent(
      "ShowPopup",
      Pointer_stringify(TitelString),
      Pointer_stringify(BodyTextString),
      Pointer_stringify(Button1Text),
      Pointer_stringify(Button2Text),
      Pointer_stringify(Button3Text),
      showX
    );
  },
  ReportRoomID: function (textStringPointer) {
    window.dispatchReactUnityEvent(
      "ReportRoomID",
      Pointer_stringify(textStringPointer)
    );
  },
  SkyboxList: function (skyboxList) {
    window.dispatchReactUnityEvent(
      "SkyboxList",
      Pointer_stringify(skyboxList)
    );
  },
  ReportLoadingStatus: function (percent) {
    window.dispatchReactUnityEvent(
      "ReportLoadingStatus",
      percent
    );
  },
});