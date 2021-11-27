using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Xrchitecture.Creator.Common.Data
{
    internal abstract class ACustomItemBehaviour : MonoBehaviour
    {
        public abstract void Initialize(List<ItemCustomArgs> args);

        public abstract ItemCustomArgs GetCustomArgs();
    }
}