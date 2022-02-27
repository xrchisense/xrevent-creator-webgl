using System;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class XrCreatorUtility
    {
       
        public static GameObject CreateRoomGameObject(RoomContainer roomToCreate)
        {
            //+1 to wait for the RoomObject to be returned. As the reference is needed for finishing loading!.
            CreatorSessionManager.ObjectsToLoad = roomToCreate.Items.Count + 1;
            //Report Loading Start to LevelMananger
            HelperBehaviour.Instance.LevelManager.ReportLoadingStatus(0);
            
            //Create Basis
            Transform roomRoot = new GameObject().transform;
            roomRoot.name = roomToCreate.Name;
            roomRoot.tag = "EventItem";
            
            //Start Loading
            foreach (ItemContainer itemContainer in roomToCreate.Items)
            {
                try
                {
                    CreateItem(itemContainer, gO =>
                    {
                        OnItemCreated(gO, itemContainer, roomRoot,false);
                        CreatorSessionManager.TrackLoadingStatus(1);
                    });
                }
                catch (Exception e)
                {
                    HelperBehaviour.Instance.LevelManager.ShowPopUp("Error Loading Model:" + itemContainer.ResourceName,e.ToString(),"continue",x => {});
                    Debug.Log("Error caught outside of Loader! This should never happen.");
                    CreatorSessionManager.TrackLoadingStatus(1);
                }
                
            }
            return roomRoot.gameObject;
        }

        
        public static void SpawnItemInRoom(string itemToAdd, string itemType, GameObject currentRoomGameObject, Action<ItemContainer> onSuccess)
        {
            ItemContainer newItemContainer = new ItemContainer()
            {
                ResourceName = itemToAdd,
                ItemType = itemType,
                Scale = new Vector3(1,1,1)
            };
            CreateItem(newItemContainer , createdObject =>
            {
                OnItemCreated(createdObject, newItemContainer, currentRoomGameObject.transform, true);
                HelperBehaviour.Instance.InputManager.SelectItem(createdObject);
            });
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
            }else if (itemContainerToCreate.ItemType == "room")
            {
                
                CreateRoomGameObject(itemContainerToCreate, onSuccess);
            }
            
        }

        private static void OnItemCreated(GameObject item, ItemContainer itemContainer, Transform roomRoot, bool showObject)
        {
            GameObject itemRoot = new GameObject();
            itemRoot.name = itemContainer.ResourceName + "-ROOT";
            item.transform.SetParent(itemRoot.transform);
            itemRoot.transform.SetParent(roomRoot);
            itemRoot.SetActive(showObject);
            
            CreatorItem creatorItem = itemRoot.AddComponent<CreatorItem>();
            //goes through every Child and looks for meshes to put Collider on
            foreach (MeshRenderer mesh in item.GetComponentsInChildren<MeshRenderer>()) {
                  if (!mesh.TryGetComponent<Collider>(out var component))
                  {
                        mesh.gameObject.AddComponent<MeshCollider>();
                        if (itemContainer.ItemType == "room") {mesh.gameObject.layer = 11;}
                        
                  }
                  else
                  {
                      //TODO: Idea for making normal Collider and not Mesh Collider !
                      //component.bounds.Contains(mesh.bounds.extents);
                  }
            }

            if (itemContainer.ItemType == "room") {CreatorSessionManager.SetVenueGameObject(itemRoot);}
            creatorItem.Initialize(itemContainer);
        }

        private static void CreateUserGameObject(ItemContainer userItemContainerToCreate, Action<GameObject> onSuccess)
        {
            string extension = System.IO.Path.GetExtension(userItemContainerToCreate.ResourceName);

            
            //TODO: This is still not perfect as the TestConfigHelper is used !
            //
            //and the Debugger does not work:
            
            
            if (extension == ".glb" || extension == ".gltf")
            {
                // ToDo: ProjectId (GUID)path is still hardcoded. needs to change!
                GLTFLoader.CreateModelFromAddress(
                    TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId +
                    "/items/" + userItemContainerToCreate.ResourceName, onSuccess);
            }
            
        }
        private static void CreateRoomGameObject(ItemContainer userItemContainerToCreate, Action<GameObject> onSuccess)
        {
            string extension = System.IO.Path.GetExtension(userItemContainerToCreate.ResourceName);

            
            //TODO: This is still not perfect as the TestConfigHelper is used !
            //
            //and the Debugger does not work:
            
            
            if (extension == ".glb" || extension == ".gltf")
            {
                // ToDo: ProjectId (GUID)path is still hardcoded. needs to change!
                GLTFLoader.CreateModelFromAddress(
                    TestConfigHelper.ProjectDataFolder + TestConfigHelper.ProjectId +
                    "/rooms/" + userItemContainerToCreate.ResourceName, onSuccess);
            }
            
        }

        private static void CreatePredefinedGameObject(
            ItemContainer predefinedItemContainerToCreate, Action<GameObject> onSuccess)
        {
            //The prefabs can be set in the WebGLConnectorUI.


            GameObject itemPrefab =
                TestConfigHelper.PrefabList.Find(x => x.name == predefinedItemContainerToCreate.ResourceName);
            
            GameObject createdObject = Object.Instantiate(itemPrefab);
            
            onSuccess(createdObject);
        }
    }
}