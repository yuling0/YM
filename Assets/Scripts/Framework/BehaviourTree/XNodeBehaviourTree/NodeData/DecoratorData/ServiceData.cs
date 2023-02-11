using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class ServiceData : NodeDataBase
{
    public string eventName;
    public float checkInterval;

    public override Decorator CreateDecorator()
    {
        Service service = ReferencePool.Instance.Acquire<Service>();
        service.interval = checkInterval;
        service.eventName = eventName;
        return service;
    }

}
