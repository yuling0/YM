using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class ConditionData : NodeDataBase
{
    public string stopPolicy;
    public string eventName;
    public float checkInterval;
    public override Decorator CreateDecorator()
    {
        Condition condition = ReferencePool.Instance.Acquire<Condition>();
        condition.eventName = eventName;
        condition.stopPolicy = Extension.Parse<E_StopPolicy>(stopPolicy);
        condition.checkInterval = checkInterval;
        return condition;
    }
}
