using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Trigger : XN_ObservingDecorator
    {
        [LabelText("检测间隔")]
        public float interval;
        [LabelText("条件函数名")]
        public string eventName;
        protected override bool IsConditionMet()
        {
            throw new System.NotImplementedException();
        }

        protected override void StartObserving()
        {
            throw new System.NotImplementedException();
        }

        protected override void StopObserving()
        {
            throw new System.NotImplementedException();
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new TriggerData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator,
                interval = interval,
                stopPolicy = stopPolicy.ToString(),
                eventName = eventName
            };
        }

        public override Node CreateNode()
        {
            Trigger trigger = ReferencePool.Instance.Acquire<Trigger>();
            trigger.interval= interval;
            trigger.eventName = eventName;
            return trigger;
        }
    }
}

