using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class WaitUntilStoppedData : NodeDataBase
{
    public bool result;
    public override Task CreateTask()
    {
        WaitUntilStopped wait = ReferencePool.Instance.Acquire<WaitUntilStopped>();
        wait.result = result;
        return wait;
    }
}
