using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class BlackboardTriggerData : NodeDataBase
{
    public string key;
    public string stopPolicy;

    public override Decorator CreateDecorator()
    {
        BlackboardTrigger trigger = ReferencePool.Instance.Acquire<BlackboardTrigger>();
        trigger.key = key;
        trigger.stopPolicy = Extension.Parse<E_StopPolicy>(stopPolicy);
        return trigger;
    }
}
