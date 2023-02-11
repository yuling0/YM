using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using YMFramework.BehaviorTree;

/// <summary>
/// 节点数据的基类（用于行为树的序列化）反序列化使用引用池创建运行时的节点
/// </summary>
[System.Serializable]
public class NodeDataBase
{
    public enum E_NodeType
    {
        Composite,
        Decorator,
        Task
    }

    public E_NodeType NodeType;     //标记该节点的类型

    public int id;  //id用来确认父子关系

    public List<int> childrenId = new List<int>();  //该节点的子节点的id


    public void AddChild(int id)
    {
        childrenId.Add(id);
    }

    public virtual Composite CreateComposite()
    {
        return null;
    }

    public virtual Decorator CreateDecorator()
    {
        return null;
    }

    public virtual Task CreateTask()
    {
        return null;
    }
}
