using System;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class XrCreatorUtility
    {
        public static GameObject CreateRoomGameObject(RoomContainer roomToCreate)
        {
            Transform roomRoot = new GameObject().transform; 

            roomRoot.name = roomToCreate.Name;
            
            foreach (ItemContainer itemContainer in roomToCreate.Items)
            {
                CreateItem(itemContainer, gO => OnItemCreated(gO, itemContainer, roomRoot));
            }

            return roomRoot.gameObject;
        }

        private static void CreateItem(ItemContainer itemContainerToCreate, Action<GameObject> onSuccess)
        {

            if (itemContainerToCreate.ItemType == "user-defined")
            {
                CreateUserGameObject(itemContainerToCreate, onSuccess);
            }
            else if (itemContainerToCreate.ItemType == "pre-defined")
            {
                CreatePredefinedGameObject(itemContainerToCreate, onSuccess);
            }
            
        }

        private static void OnItemCreated(GameObject item, ItemContainer itemContainer, Transform roomRoot = null)
        {
            GameObject itemRoot = new GameObject();
            itemRoot.name = itemContainer.ResourceName + "-ROOT";
            item.transform.SetParent(itemRoot.transform);
            itemRoot.transform.SetParent(roomRoot);
            
            CreatorItem creatorItem = itemRoot.AddComponent<CreatorItem>();
            creatorItem.Initialize(itemContainer);
        }

        private static GameObject CreateUserGameObject(ItemContainer userItemContainerToCreate, Action<GameObject> onSuccess)
        {
            GameObject createdUserObject = null;

            string? extension = System.IO.Path.GetExtension(userItemContainerToCreate.ResourceName);

            if (extension == ".glb" || extension == ".gltf")
            {
                GLTFLoader.CreateModelFromAddress(
                    TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId +
                    "/items/" + userItemContainerToCreate.ResourceName, onSuccess);
            }

            return createdUserObject;
        }

        private static GameObject CreatePredefinedGameObject(
            ItemContainer predefinedItemContainerToCreate, Action<GameObject> onSuccess)
        {
            // Using Resources right now, might switch to AssetBundles at some point. 
            // https://docs.unity3d.com/Manual/webgl-embeddedresources.html
            // -> Needs to be enabled for WebGL Embedded Resources

            GameObject itemPrefab = Resources.Load(
                TestConfigHelper.PredefinedItemsResourcePath +
                predefinedItemContainerToCreate.ResourceName) as GameObject;

            GameObject createdObject = GameObject.Instantiate(itemPrefab);

            onSuccess(createdObject);

            return createdObject;
        }
    }
}