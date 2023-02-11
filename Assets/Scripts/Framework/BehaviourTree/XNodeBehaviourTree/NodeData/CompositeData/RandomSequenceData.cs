using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class RandomSequenceData : NodeDataBase
{
    public override Composite CreateComposite()
    {
        return ReferencePool.Instance.Acquire<RandomSequence>();
    }
}
