using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_BlackboardTrigger : XN_ObservingDecorator
    {
        [LabelText("观察的键值")]
        public string key;

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
            return new BlackboardTriggerData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator,
                stopPolicy = stopPolicy.ToString(),
                key = key
            };
        }

        public override Node CreateNode()
        {
            BlackboardTrigger blackboardCondition = ReferencePool.Instance.Acquire<BlackboardTrigger>();
            blackboardCondition.stopPolicy = stopPolicy;
            blackboardCondition.key = key;
            return blackboardCondition;
        }
    }
}

