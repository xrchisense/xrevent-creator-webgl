using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;

namespace Xrchitecture.Creator.Common.Data {
    internal class DMXLampItemBehaviour : ACustomItemBehaviour
    {
        private string dmxValueName = "DMX_VALUE";
        
        public int Address {
            get
            {
                return _Address;
            }
            private set
            {
                _Address = value;
                OnCustomParameterChanged(new ItemCustomArgs(dmxValueName, _Address.ToString()));
            }
        }

        private int _Address = -1;

        public override void Initialize(List<ItemCustomArgs> args)
        {
            foreach (ItemCustomArgs arg in args)
            {
                if (arg.Argument == dmxValueName)
                {
                    Address = Int32.Parse(arg.Value);
                }
            }
        }
        
        public override void UpdateCustomArgs(string itemName, string key, string value)
        {
            if (itemName != "DmxLamp") return;
            
        }
    }
}