using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class AnimationActionData : NodeDataBase
{
    public string animationName;
    public override Task CreateTask()
    {
        AnimationAction action = ReferencePool.Instance.Acquire<AnimationAction>();
        action.animationName = animationName;
        return action;
    }
}
