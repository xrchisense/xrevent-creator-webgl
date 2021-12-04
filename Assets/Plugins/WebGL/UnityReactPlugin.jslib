mergeInto(LibraryManager.library, {
  ItemInfo: function (itemName, itemID) {
    window.dispatchReactUnityEvent(
      "ItemInfo",
      Pointer_stringify(itemName),
      itemID
    );
  },
  ShowPopup: function (textStringPointer) {
    window.dispatchReactUnityEvent(
      "ShowPopup",
      Pointer_stringify(textStringPointer)
    );
  },
});