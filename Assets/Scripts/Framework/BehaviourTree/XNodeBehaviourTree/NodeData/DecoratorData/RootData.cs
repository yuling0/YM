using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

[System.Serializable]
public class RootData : NodeDataBase
{
    public override Decorator CreateDecorator()
    {
        return ReferencePool.Instance.Acquire<Root>();
    }
}
