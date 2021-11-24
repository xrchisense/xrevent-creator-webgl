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

            foreach (ItemContainer itemToCreate in roomToCreate.Items)
            {
                GameObject itemRoot = CreateItem(itemToCreate);

                if (itemRoot != null)
                {
                    itemRoot.transform.SetParent(roomRoot);

                    itemRoot.GetComponent<CreatorItem>().Initialize(itemToCreate.ItemCustomArgs);
                }
            }
        }

        private static GameObject CreateItem(ItemContainer itemContainerToCreate)
        {
            GameObject itemRoot = new GameObject();
            itemRoot.AddComponent<CreatorItem>();
            itemRoot.name = itemContainerToCreate.ResourceName + "-ROOT";


            GameObject createdObject = null;

            if (itemContainerToCreate.ItemType == "user-defined")
            {
                createdObject = CreateUserItemGameObject(itemContainerToCreate);
            }
            else if (itemContainerToCreate.ItemType == "pre-defined")
            {
                createdObject =
                    CreatePredefinedItemGameObject(itemContainerToCreate);
            }

            if (createdObject != null)
            {
                createdObject.transform.SetParent(itemRoot.transform);
                itemRoot.transform.SetPositionAndRotation(
                    itemContainerToCreate.Position,
                    itemContainerToCreate.Rotation);
            }

            return itemRoot;
        }

        private static GameObject CreateUserItemGameObject(ItemContainer userItemContainerToCreate)
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

        private static GameObject CreatePredefinedItemGameObject(
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