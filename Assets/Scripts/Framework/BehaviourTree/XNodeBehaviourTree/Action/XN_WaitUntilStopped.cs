using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_WaitUntilStopped : XN_Task
    {
        [LabelText("停止时返回的结果")]
        public bool result;
        //public XN_WaitUntilStopped(bool result = false) : base("WaitUntilStopped")
        //{
        //    this.result = result;
        //}

        protected override void DoStop()
        {
            Stopped(result);
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new WaitUntilStoppedData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Task,
                result = result
            };
        }

        public override Node CreateNode()
        {
            WaitUntilStopped waitUntilStopped = ReferencePool.Instance.Acquire<WaitUntilStopped>();
            waitUntilStopped.result = result;
            return waitUntilStopped;
        }
    }
}

