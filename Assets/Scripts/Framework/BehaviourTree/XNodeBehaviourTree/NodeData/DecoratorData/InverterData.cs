using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class InverterData : NodeDataBase
{
    public override Decorator CreateDecorator()
    {
        return ReferencePool.Instance.Acquire<Inverter>();
    }
}
