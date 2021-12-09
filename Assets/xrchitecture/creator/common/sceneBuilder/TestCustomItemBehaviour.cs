using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    class TestCustomItemBehaviour : ACustomItemBehaviour
    {
        private List<ItemCustomArgs> _itemCustomArgs = new List<ItemCustomArgs>();

        public override void Initialize(List<ItemCustomArgs> args)
        {
            string argsString = "";
            for (int i = 0; i < args.Count; i++)
            {
                argsString += ("{" + args[i].Argument + " " + args[i].Value + "} ");
            }

            Debug.Log("Initialized Test-Item with args: " + Environment.NewLine +
                      argsString);


            _itemCustomArgs = args;

            StartCoroutine(ChangeCustomArgs());
        }


        private IEnumerator ChangeCustomArgs()
        {
            while (true)
            {
                yield return new WaitForSeconds(1.0f);

                for (int i = 0; i < _itemCustomArgs.Count; i++)
                {
                    OnCustomParameterChanged(_itemCustomArgs[i]); // does not actually change anything - implemented as a test
                }
            }
        }
    }
}