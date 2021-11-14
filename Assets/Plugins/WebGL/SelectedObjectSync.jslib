mergeInto(LibraryManager.library, {
  SpawnItem: function (type, score) {
    dispatchReactUnityEvent("SpawnItemEvent", Pointer_stringify(type), score);
  },
});