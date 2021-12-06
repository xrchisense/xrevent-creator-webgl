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
                OnCustomParameterChanged(new ItemCustomPar(dmxValueName, _Address.ToString()));
            }
        }

        private int _Address = -1;

        public override void Initialize(List<ItemCustomPar> args)
        {
            foreach (ItemCustomPar arg in args)
            {
                if (arg.Argument == dmxValueName)
                {
                    Address = Int32.Parse(arg.Value);
                }
            }
        }
    }
}