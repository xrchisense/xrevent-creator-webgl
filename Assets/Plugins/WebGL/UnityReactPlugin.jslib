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
  ShowPopup: function (textStringPointer) {
    window.dispatchReactUnityEvent(
      "ShowPopup",
      Pointer_stringify(textStringPointer)
    );
  },
  ReportRoomID: function (textStringPointer) {
    window.dispatchReactUnityEvent(
      "ReportRoomID",
      Pointer_stringify(textStringPointer)
    );
  },
});