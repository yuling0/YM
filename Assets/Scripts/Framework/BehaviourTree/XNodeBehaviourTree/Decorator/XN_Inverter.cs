using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Inverter : XN_Decorator
    {
        //public XN_Inverter(XN_NodeBase decorator) : base("Inverter", decorator)
        //{

        //}

        protected override void DoStart()
        {
            decoratee.Start();
        }

        protected override void DoStop()
        {
            decoratee.Stop();
        }

        protected override void DoChildStopped(XN_NodeBase child, bool result)
        {
            Stopped(!result);
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new InverterData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator
            };
        }

        public override Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<Inverter>();
        }
    }
}