using System;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class RoomCreatorUtility
    {
        public static void CreateRoomGameObject(RoomContainer roomToCreate)
        {
            Transform roomRoot = new GameObject().transform; 

            roomRoot.name = roomToCreate.Name;

            foreach (ItemContainer itemContainer in roomToCreate.Items)
            {
                CreatorItem creatorItem;
                GameObject itemRoot = CreateItem(itemContainer, out creatorItem);

                if (itemRoot != null)
                {
                    itemRoot.transform.SetParent(roomRoot);

                    creatorItem.Initialize(itemContainer);
                }
            }
        }

        private static GameObject CreateItem(ItemContainer itemContainerToCreate, out CreatorItem creatorItem)
        {
            GameObject itemRoot = new GameObject();
            itemRoot.name = itemContainerToCreate.ResourceName + "-ROOT";
            creatorItem = itemRoot.AddComponent<CreatorItem>();

            GameObject createdObject = null;

            if (itemContainerToCreate.ItemType == "user-defined")
            {
                createdObject = CreateUserGameObject(itemContainerToCreate);
            }
            else if (itemContainerToCreate.ItemType == "pre-defined")
            {
                createdObject = CreatePredefinedGameObject(itemContainerToCreate);
            }

            if (createdObject != null)
            {
                createdObject.transform.SetParent(itemRoot.transform);
            }

            return itemRoot;
        }

        private static GameObject CreateUserGameObject(ItemContainer userItemContainerToCreate)
        {
            GameObject createdUserObject = null;

            string? extension = System.IO.Path.GetExtension(userItemContainerToCreate.ResourceName);

            if (extension == ".glb" || extension == ".gltf")
            {
                createdUserObject = GLTFLoader.CreateModelFromAddress(
                    TestConfigHelper.UserDataFolder + TestConfigHelper.UserId +
                    "/Items/" + userItemContainerToCreate.ResourceName);
            }

            return createdUserObject;
        }

        private static GameObject CreatePredefinedGameObject(
            ItemContainer predefinedItemContainerToCreate)
        {
            // Using Resources right now, might switch to AssetBundles at some point. 
            // https://docs.unity3d.com/Manual/webgl-embeddedresources.html
            // -> Needs to be enabled for WebGL Embedded Resources

            GameObject itemPrefab = Resources.Load(
                TestConfigHelper.PredefinedItemsResourcePath +
                predefinedItemContainerToCreate.ResourceName) as GameObject;

            GameObject createdObject = GameObject.Instantiate(itemPrefab);

            return createdObject;
        }
    }
}