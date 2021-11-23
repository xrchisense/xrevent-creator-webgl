using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class RoomCreatorUtility
    {
        public static void CreateRoomGameObject(Room roomToCreate)
        {
            Transform
                roomRoot =
                    new GameObject()
                        .transform; // creates a gameobject at (0,0,0) with identity rotation

            roomRoot.name = roomToCreate.Name;

            foreach (Item itemToCreate in roomToCreate.Items)
            {
                GameObject item = null;

                try
                {
                    item = CreateItem(itemToCreate);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
                
                if (item != null)
                {
                    Transform itemRoot = new GameObject().transform;
                    itemRoot.name = itemToCreate.ResourceName + "-ROOT";
                    
                    item.transform.SetParent(itemRoot);

                    itemRoot.SetPositionAndRotation(itemToCreate.Position, itemToCreate.Rotation);
                    itemRoot.SetParent(roomRoot.transform);
                }
            }
        }

        [CanBeNull]
        private static GameObject CreateItem(Item itemToCreate)
        {
            GameObject createdObject = null;

            if (itemToCreate.ItemType == "user-defined")
            {
                createdObject = CreateUserObject(itemToCreate);
            }
            else if (itemToCreate.ItemType == "pre-defined")
            {
                createdObject = CreatePredefinedObject(itemToCreate);
            }

            return createdObject;
        }

        private static GameObject CreateUserObject(Item userItemToCreate)
        {
            return null;
        }

        private static GameObject CreatePredefinedObject(Item predefinedItemToCreate)
        {
            // Using Resources right now, might switch to AssetBundles at some point. 
            // https://docs.unity3d.com/Manual/webgl-embeddedresources.html
            // -> Needs to be enabled for WebGL Embedded Resources
            
            GameObject itemPrefab = Resources.Load(
                TestConfigHelper.PredefinedItemsResourcePath +
                predefinedItemToCreate.ResourceName) as GameObject;

            GameObject createdObject = GameObject.Instantiate(itemPrefab);

            return createdObject;
        }
    }
}