using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTreeEditor
{
    public abstract class XN_Task : XN_NodeBase
    {
        //public XN_Task(string name) : base(name)
        //{

        //}

        public override void OnRemoveConnection(NodePort from, NodePort to)
        {
            SetParent(null);
        }
    }
}