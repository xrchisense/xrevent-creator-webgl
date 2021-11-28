// ------------------------------------------------------------------------------------------------------
// <copyright file="CreatorProjectManager.cs" company="Monstrum">
// Copyright (c) 2021 Monstrum. All rights reserved.
// </copyright>
// <author>
//   Nikolai Reinke
// </author>
// ------------------------------------------------------------------------------------------------------

using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal static class CreatorEventManager
    {
        private static XrEventContainer _currentEvent;

        private static GameObject _currentRoomGameObject;

        public static void SetCreatorEvent(XrEventContainer xrEvent)
        {
            GameObject.Destroy(_currentRoomGameObject);
            
            _currentEvent = xrEvent;
            
            _currentRoomGameObject = XrCreatorUtility.CreateRoomGameObject(_currentEvent.Rooms[0]);
        }

        public static void SaveEventData()
        {
            
        }
    }
}