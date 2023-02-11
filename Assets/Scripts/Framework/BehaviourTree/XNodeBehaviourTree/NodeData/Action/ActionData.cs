using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class ActionData : NodeDataBase
{
    public string eventName;

    public override Task CreateTask()
    {
        Action action = ReferencePool.Instance.Acquire<Action>();
        action.eventName = eventName;
        return action;
    }
}
