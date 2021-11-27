using System;
using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    class TestCustomItemBehaviour : ACustomItemBehaviour
    {
        public override void Initialize(List<ItemCustomArgs> args)
        {
            string argsString = "";
            for (int i = 0; i < args.Count; i++)
            {
                argsString += ("{" + args[i].Argument + " " + args[i].Value + "} ");
            }

            Debug.Log("Initialized Test-Item with args: " + Environment.NewLine +
                      argsString);
        }

        public override ItemCustomArgs GetCustomArgs()
        {
            throw new NotImplementedException();
        }
    }
}