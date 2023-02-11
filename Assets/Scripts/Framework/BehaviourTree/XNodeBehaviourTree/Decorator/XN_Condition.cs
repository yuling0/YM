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
        [LabelText("条件函数")]
        public Func<bool> condition;
        [LabelText("检查间隔")]
        public float checkInterval;

        [LabelText("条件函数名")]
        public string eventName;
        public override void OnInit()
        {
            Delegate d = GetDelegate(eventName);
            if (d == null)
            {
                Debug.Log("未找到对应的函数");
            }
            if (d is Func<bool>)
            {
                condition = d as Func<bool>;
            }
            else
            {
                Debug.Log("该函数不属于该委托类型");
            }
            
        }
        //public XN_Condition(E_StopPolicy stopPolicy, Func<bool> condition, float checkInterval, XN_NodeBase decorator) : base("Condition", stopPolicy, decorator)
        //{
        //    this.condition = condition;
        //    this.checkInterval = checkInterval;
        //}

        protected override bool IsConditionMet()
        {
            Debug.Log($"执行条件结果{condition.Invoke()}");
            return condition.Invoke();
        }

        protected override void StartObserving()
        {
            Debug.Log($"间隔{checkInterval}");
            Debug.Log($"条件函数{condition}");

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