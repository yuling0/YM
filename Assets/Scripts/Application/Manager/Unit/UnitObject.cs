using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

public class UnitObject : ObjectBase
{
    public static UnitObject Create(string unitName, object objectInstance , GameObject parent)
    {
        UnitObject unitObject = ReferencePool.Instance.Acquire<UnitObject>();
        unitObject.Init(unitName, objectInstance);
        (unitObject.Target as GameObject).transform.parent = parent.transform;
        return unitObject;
    }
}
