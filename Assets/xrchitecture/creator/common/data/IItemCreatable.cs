using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Xrchitecture.Creator.Common.Data;

internal interface IItemCreatable
{
    void ApplyCustomArguments(params ItemCustomArgs[] args);
}
