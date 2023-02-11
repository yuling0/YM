using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using YMFramework.BehaviorTreeEditor;

public class XN_PrefabAction : XN_Task
{
    public GameObject prefab;

    public override NodeDataBase CreateNodeData(int id)
    {
        return new PrefabActionData()
        {
            id = id,
            NodeType = NodeDataBase.E_NodeType.Task,
            prefab = prefab
        };
    }
    public override Node CreateNode()
    {
        PrefabAction prefabAction = ReferencePool.Instance.Acquire<PrefabAction>();
        prefabAction.prefab = prefab;
        return prefabAction;
    }
}
