using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class PrefabActionData : NodeDataBase
{
    [OdinSerialize]
    public GameObject prefab;
    public override Task CreateTask()
    {
        PrefabAction pa = ReferencePool.Instance.Acquire<PrefabAction>();
        pa.prefab = prefab;
        return pa;
    }
}
