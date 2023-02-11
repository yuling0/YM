using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class ParallelData : NodeDataBase
{
    public string successPolicy;
    public string failurePolicy;
    public override Composite CreateComposite()
    {
        Parallel parallel = ReferencePool.Instance.Acquire<Parallel>();
        parallel.successPolicy = Extension.Parse<E_Policy>(successPolicy);
        parallel.failurePolicy = Extension.Parse<E_Policy>(failurePolicy);
        return parallel;
    }
}
