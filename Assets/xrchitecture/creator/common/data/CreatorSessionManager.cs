// ------------------------------------------------------------------------------------------------------
// <copyright file="CreatorProjectManager.cs" company="Monstrum">
// Copyright (c) 2021 Monstrum. All rights reserved.
// </copyright>
// <author>
//   Nikolai Reinke
// </author>
// ------------------------------------------------------------------------------------------------------

using Newtonsoft.Json;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class CreatorSessionManager
    {
        private static XrEventContainer _currentEvent;
        private static GameObject _currentRoomGameObject;

        public static void SetCreatorEvent(XrEventContainer xrEvent)
        {
            _currentEvent = xrEvent;
            
            SetCurrentRoom(_currentEvent.Rooms[0]);
        }

        public static XrEventContainer GetCreatorEvent()
        {
            return _currentEvent;
        }

        public static void SetCurrentRoom(RoomContainer roomContainer)
        {
            DestroyRoomGameObject();
            
            _currentRoomGameObject = XrCreatorUtility.CreateRoomGameObject(roomContainer);
        }

        private static void DestroyRoomGameObject()
        {
            if (_currentRoomGameObject != null)
            {
                GameObject.Destroy(_currentRoomGameObject);
                _currentRoomGameObject = null;
            }
        }
    }
}