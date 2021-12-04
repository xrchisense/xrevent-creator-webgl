mergeInto(LibraryManager.library, {
  SpawnItem: function (type, score) {
    window.dispatchReactUnityEvent("SpawnItemEvent", Pointer_stringify(type), score);
  },
  
  reportRoomID: function(eventID) {
	window.dispatchReactUnityEvent("InitRoomID", Pointer_stringify(eventID));
  },
  
});