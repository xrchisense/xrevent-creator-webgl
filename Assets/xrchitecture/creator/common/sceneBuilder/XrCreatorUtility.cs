using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class XrCreatorUtility
    {
        public static GameObject CreateRoomGameObject(RoomContainer roomToCreate)
        {
            Transform roomRoot = new GameObject().transform; 

            roomRoot.name = roomToCreate.Name;

            roomRoot.tag = "EventItem";
            
            foreach (ItemContainer itemContainer in roomToCreate.Items)
            {
                CreateItem(itemContainer, gO => OnItemCreated(gO, itemContainer, roomRoot));
            }

            return roomRoot.gameObject;
        }
        
        public static void AddItemToCurrentRoom(string itemToAdd, string itemType, GameObject currentRoomGameObject, Action<ItemContainer> onSuccess)
        {
            ItemContainer newItemContainer = new ItemContainer()
            {
                ResourceName = itemToAdd,
                ItemType = itemType
            };
            Debug.Log(currentRoomGameObject);
            CreateItem(newItemContainer , createdObject => OnItemCreated(createdObject, newItemContainer, currentRoomGameObject.transform));
            onSuccess(newItemContainer);
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
            
            //Add Collider to spawned Object when no Collider but a Mesh is present.
            //Does not solve the Problem when there is another Parent Root GameObject, then no collider will be applied.
            //This should maybe be moved to the CreatorItem.init
            if (!item.TryGetComponent<Collider>(out var component) && item.TryGetComponent<MeshRenderer>(out var meshRenderer))
            {
                item.AddComponent<MeshCollider>();
            }
            
            
            CreatorItem creatorItem = itemRoot.AddComponent<CreatorItem>();
            creatorItem.Initialize(itemContainer);
        }

        private static void CreateUserGameObject(ItemContainer userItemContainerToCreate, Action<GameObject> onSuccess)
        {
            GameObject createdUserObject = null;

            string? extension = System.IO.Path.GetExtension(userItemContainerToCreate.ResourceName);

            
            //TODO: This is still not perfect as the TestConfigHelper is used !
            //
            //and the Debugger does not work:
            
            
            if (extension == ".glb" || extension == ".gltf")
            {
                GLTFLoader.CreateModelFromAddress(
                    TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId +
                    "/items/" + userItemContainerToCreate.ResourceName, onSuccess);
            }
            
        }

        private static void CreatePredefinedGameObject(
            ItemContainer predefinedItemContainerToCreate, Action<GameObject> onSuccess)
        {
            
            
            // Using Prefabs right now, might switch to AssetBundles at some point. 
            // https://docs.unity3d.com/Manual/webgl-embeddedresources.html
            // -> Needs to be enabled for WebGL Embedded Resources
            
            //The prefabs can be set in the WebGLConnectorUI.


            GameObject itemPrefab =
                TestConfigHelper.PrefabList.Find(x => x.name == predefinedItemContainerToCreate.ResourceName);
            
            GameObject createdObject = Object.Instantiate(itemPrefab);
            
            onSuccess(createdObject);
        }

        
    }
}