using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class WaitData : NodeDataBase
{
    public float seconds;
    public override Task CreateTask()
    {
        Wait wait = ReferencePool.Instance.Acquire<Wait>();
        wait.seconds = seconds;
        return wait;
    }
}
