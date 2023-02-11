using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class TriggerData : NodeDataBase
{
    public float interval;
    public string eventName;
    public string stopPolicy;
    public override Decorator CreateDecorator()
    {
        Trigger trigger = ReferencePool.Instance.Acquire<Trigger>();
        trigger.interval = interval;
        trigger.eventName = eventName;
        trigger.stopPolicy = Extension.Parse<E_StopPolicy>(stopPolicy);
        return trigger;
    }
}
