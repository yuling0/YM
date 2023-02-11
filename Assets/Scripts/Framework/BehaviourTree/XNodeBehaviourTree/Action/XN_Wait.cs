using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Wait : XN_Task
    {
        [LabelText("µÈ´ýÊ±¼ä")]
        public float seconds;
        //public XN_Wait(float seconds) : base("Wait")
        //{
        //    this.seconds = seconds;
        //}

        protected override void DoStart()
        {
            this.Root.Clock.AddTimer(seconds, 0, OnTime);
        }

        protected override void DoStop()
        {
            this.Root.Clock.RemoveTimer(OnTime);
            Stopped(false);
        }

        private void OnTime()
        {

            Stopped(true);
        }
        public override NodeDataBase CreateNodeData(int id)
        {
            return new WaitData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Task,
                seconds = seconds
            };
        }

        public override Node CreateNode()
        {
            Wait wait = ReferencePool.Instance.Acquire<Wait>();
            wait.seconds= seconds;
            return wait;
        }
    }

}
