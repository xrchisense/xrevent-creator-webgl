using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class CreatorSessionManager
    {
        private static XrEventContainer _currentEvent;
        private static GameObject _currentRoomGameObject;

        private static int _objectsLoaded;
        public static int ObjectsToLoad;
        
        
        //get Functions:
        public static XrEventContainer GetCreatorEvent()
        {
            return _currentEvent;
        }

        public static GameObject GetCurrentRoomGameObject()
        {
            return _currentRoomGameObject;
        }
        
        //Loading / unloading Rooms:
        //
        //Loading Empty Event:
        public static void CreateNewCreatorEvent(string guid, List<ItemContainer> defaultItems = null)
        {
            //add default Items to ItemContainerList
            List<ItemContainer> defaultItemList = defaultItems;
            if (defaultItems == null)
            {
                defaultItemList = new List<ItemContainer>();
                //defaultItemList.Add(new ItemContainer(){ItemType = "pre-defined",ResourceName = "Plane"});
                defaultItemList.Add(new ItemContainer(){ItemType = "pre-defined",ResourceName = "Directional Light", Position = new Vector3(0,3,0)});
            }
            
            //Create Default Event
            XrEventContainer xrc = new XrEventContainer()
            {
                Name = "EmptyRoom",
                JsonVersion = HelperBehaviour.Instance.currentJsonVersion,
                Rooms = new RoomContainer[]
                {
                    new RoomContainer()
                    {
                        Name = "DefaultRoom",
                        Guid = guid,
                        Items = defaultItemList,
                        Skybox = "Day_BlueSky_Nothing"
                    }
                }
            };
            SetCreatorEvent(xrc);
            
            //TODO:
            //Popup: Default Scenen/Tutorial, name etc.
            
        }
        
        public static void SetCreatorEvent(XrEventContainer xrEvent)
        {
            _currentEvent = xrEvent;

            //check Event Json Version;
            Debug.Log("Event Version: " + _currentEvent.JsonVersion + " Editor Version: " + HelperBehaviour.Instance.currentJsonVersion);
            
            if (_currentEvent.JsonVersion < HelperBehaviour.Instance.currentJsonVersion)
            {
                _currentEvent = XrJsonVersionRepair.UpdateEventContainer(_currentEvent);
                HelperBehaviour.Instance.LevelManager.ShowPopUp("Warning!","This Event was Saved with an old Version of the Creator. The program updated it, please test everything thoroughly and save! You can see the Changelog here: <a href ='www.xrchitecture.de/creator/changelog'>www.xrchitecture.de/creator/changelog</a>","Okay",null);
            }

            if (_currentEvent.JsonVersion > HelperBehaviour.Instance.currentJsonVersion)
            {
                //TODO: should the newer version be given an old number ??
                //_currentEvent = XrJsonVersionRepair.UpdateEventContainer(_currentEvent);
                //HelperBehaviour.Instance.LevelManager.ShowPopUp("Warning!","This Event was saved with a newer Version of the Creator. Please test everything thoroughly, see the changes here: <a href ='www.xrchitecture.de/creator/changelog'>www.xrchitecture.de/creator/changelog</a> ","Okay",null);
            }
            
            _objectsLoaded = 0;
            SetCurrentRoom(_currentEvent.Rooms[0]);
        }

        private static void SetCurrentRoom(RoomContainer roomContainer)
        {
            DestroyRoomGameObject();
            _currentRoomGameObject = XrCreatorUtility.CreateRoomGameObject(roomContainer);
            //when object is returned finish Loading
            TrackLoadingStatus(1);
            //Set Skybox
            HelperBehaviour.Instance.gameObject.GetComponent<CreatorLevelManager>().setSkybox(roomContainer.Skybox);
        }
        
        private static void DestroyRoomGameObject()
        {
            if (_currentRoomGameObject == null) return;
            
            Object.Destroy(_currentRoomGameObject);
            _currentRoomGameObject = null;

        }
        
        
        //Editing The Event and the Room - Parameter:
        public static void SetEventName(string value)
        {
            _currentEvent.Name = value;
        }
        public static void SetRoomName(string value)
        {
            _currentEvent.Rooms[0].Name = value;
        }
        public static void SetRoomSkybox(string value)
        {
            _currentEvent.Rooms[0].Skybox = value;
        }
        
        //Editing The Room:
        public static void SpawnItemInCurrentRoom(string itemToAdd, string itemType)
        {
            XrCreatorUtility.SpawnItemInRoom(itemToAdd, itemType, _currentRoomGameObject, container => _currentEvent.Rooms[0].Items.Add(container));
        }

        public static void RemoveItemFromCurrentRoom(CreatorItem itemToRemove)
        {
            _currentEvent.Rooms[0].Items.Remove(itemToRemove.ItemContainer);
            GameObject.Destroy(itemToRemove.gameObject);
        }
        

        //loading Status Tracker:
        
        public static void TrackLoadingStatus(int counter)
        {
            _objectsLoaded += counter;
            float itemPercent = (((float) _objectsLoaded / ObjectsToLoad)*100);
            //Reporting to UI
            HelperBehaviour.Instance.LevelManager.ReportLoadingStatus((int)itemPercent);
            
            
            //unhiding all Object
            if (_objectsLoaded >= ObjectsToLoad)
            {
                HelperBehaviour.Instance.OnFinishLoad();
            }
        }
    }
}