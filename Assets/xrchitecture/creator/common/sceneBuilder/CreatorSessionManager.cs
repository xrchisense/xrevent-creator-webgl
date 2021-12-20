using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class CreatorSessionManager
    {
        private static XrEventContainer _currentEvent;
        private static GameObject _currentRoomGameObject;

        public static void CreateNewCreatorEvent(string guid)
        {
            //add default Items to ItemContainerList
            List<ItemContainer> defaultItemList = new List<ItemContainer>();
            defaultItemList.Add(new ItemContainer(){ItemType = "pre-defined",ResourceName = "Plane"});
            defaultItemList.Add(new ItemContainer(){ItemType = "pre-defined",ResourceName = "Directional Light", Position = new Vector3(0,3,0)});
            
            //Create Default Event
            XrEventContainer xrc = new XrEventContainer()
            {
                Name = "EmptyRoom",
                Rooms = new RoomContainer[]
                {
                    new RoomContainer()
                    {
                        Guid = guid,
                        Items = defaultItemList
                    }
                }
            };
            //Set DefaultEvent an Load it. 
            SetCreatorEvent(xrc);
            
            //TODO:
            //Popup: Default Scenen/Tutorial, name etc.
            
        }
        public static void SetCreatorEvent(XrEventContainer xrEvent)
        {
            _currentEvent = xrEvent;
            
            SetCurrentRoom(_currentEvent.Rooms[0]);
        }

        public static XrEventContainer GetCreatorEvent()
        {
            return _currentEvent;
        }

        public static GameObject GetCurrentRommGameObject()
        {
            return _currentRoomGameObject;
        }

        public static void SetCurrentRoom(RoomContainer roomContainer)
        {
            DestroyRoomGameObject();

            _currentRoomGameObject = XrCreatorUtility.CreateRoomGameObject(roomContainer);
        }

        public static void SpawnItemInCurrentRoom(string itemToAdd, string itemType)
        {
            XrCreatorUtility.SpawnItemInRoom(itemToAdd, itemType, _currentRoomGameObject, container => _currentEvent.Rooms[0].Items.Add(container));
        }

        public static void RemoveItemFromCurrentRoom(CreatorItem itemToRemove)
        {
            _currentEvent.Rooms[0].Items.Remove(itemToRemove.ItemContainer);
            GameObject.Destroy(itemToRemove.gameObject);
        }
        
        private static void DestroyRoomGameObject()
        {
            if (_currentRoomGameObject != null)
            {
                GameObject.Destroy(_currentRoomGameObject);
                _currentRoomGameObject = null;
            }
            
            //Destroy all with the tag also ??
        }
    }
}