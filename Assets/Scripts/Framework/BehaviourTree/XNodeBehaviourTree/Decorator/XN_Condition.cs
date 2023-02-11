using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Condition : XN_ObservingDecorator
    {
        [LabelText("��������")]
        public Func<bool> condition;
        [LabelText("�����")]
        public float checkInterval;

        [LabelText("����������")]
        public string eventName;
        public override void OnInit()
        {
            Delegate d = GetDelegate(eventName);
            if (d == null)
            {
                Debug.Log("δ�ҵ���Ӧ�ĺ���");
            }
            if (d is Func<bool>)
            {
                condition = d as Func<bool>;
            }
            else
            {
                Debug.Log("�ú��������ڸ�ί������");
            }
            
        }
        //public XN_Condition(E_StopPolicy stopPolicy, Func<bool> condition, float checkInterval, XN_NodeBase decorator) : base("Condition", stopPolicy, decorator)
        //{
        //    this.condition = condition;
        //    this.checkInterval = checkInterval;
        //}

        protected override bool IsConditionMet()
        {
            Debug.Log($"ִ���������{condition.Invoke()}");
            return condition.Invoke();
        }

        protected override void StartObserving()
        {
            Debug.Log($"���{checkInterval}");
            Debug.Log($"��������{condition}");

            this.Root.Clock.AddTimer(checkInterval, -1, Evaluate);
        }

        protected override void StopObserving()
        {
            this.Root.Clock.RemoveTimer(Evaluate);
        }

        protected override void DoChildStopped(XN_NodeBase child, bool result)
        {

            base.DoChildStopped(child, result);
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new ConditionData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator,
                stopPolicy = stopPolicy.ToString(),
                checkInterval = checkInterval,
                eventName = eventName
            };

        }

        public override Node CreateNode()
        {
            Condition condition = ReferencePool.Instance.Acquire<Condition>();
            condition.stopPolicy = stopPolicy;
            condition.eventName = eventName;
            condition.checkInterval= checkInterval;
            condition.condition = this.condition;
            return condition;
        }
    }
}