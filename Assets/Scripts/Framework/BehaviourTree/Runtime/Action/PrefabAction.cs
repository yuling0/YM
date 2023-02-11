using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class PrefabAction : Task
{
    public GameObject prefab;
    public PrefabAction() : base("PrefabAction")
    {

    }

    protected override void DoStart()
    {
        base.DoStart();
    }
}
