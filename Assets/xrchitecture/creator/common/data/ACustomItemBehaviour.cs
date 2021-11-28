using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal abstract class ACustomItemBehaviour : MonoBehaviour
    {

        internal delegate void CustomParameterChanged(ItemCustomArg itemCustomArg);
        internal CustomParameterChanged OnCustomParameterChanged;
        
        public abstract void Initialize(List<ItemCustomArg> args);
    }
}