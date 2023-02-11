using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using YMFramework.BehaviorTreeEditor;

public class RandomSelectorData : NodeDataBase
{
    public override Composite CreateComposite()
    {
        return ReferencePool.Instance.Acquire<RandomSelector>();
    }
}
