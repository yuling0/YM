using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTreeEditor
{
    [NodeTitle("¸ù½Úµã")]
    public class XN_Root : XN_Decorator
    {
        XN_GlobalClock clock;
        XN_Blackboard blackboard;
        public XN_GlobalClock Clock => clock;

        public XN_Blackboard Blackboard => blackboard;

        //public XN_Root(XN_NodeBase decorator) : base("Root", decorator)
        //{
        //    clock = XN_GlobalClock.Instance;
        //    blackboard = clock.GetBlackboard();
        //    SetRoot(this);
        //}
        public override void OnInit()
        {
            clock = XN_GlobalClock.Instance;
            blackboard = clock.GetBlackboard();
            SetRoot(this);
        }
        public override void SetRoot(XN_Root root)
        {
            decoratee.SetRoot(this);
        }

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
            if(!IsStopRequested)
            {
                clock.AddTimer(0, 0, Start);
            }
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new RootData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator
            };
        }

        public override BehaviorTree.Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<BehaviorTree.Root>();
        }

    }
}

