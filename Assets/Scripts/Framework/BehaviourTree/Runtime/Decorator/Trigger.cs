using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    /// <summary>
    /// 触发器：一旦满足条件就会执行到底（当条件函数从true到false时，也会执行到底）
    /// </summary>
    public class Trigger : ObservingDecorator
    {
        public float interval;
        public string eventName;
        Func<bool> func;


        public Trigger() : base("Trigger")
        {

        }
        public override void OnInit()
        {
            base.OnInit();
            func = GetDelegate(eventName) as Func<bool>;
        }

        protected override bool IsConditionMet()
        {
            return func.Invoke();
        }

        protected override void Evaluate()
        {
            if (!IsActive && IsConditionMet()) //此节点未启用，并且当前条件满足
            {
                if (stopPolicy == E_StopPolicy.AbortParent || stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                {
                    Container parent = this.ParentNode;
                    Node childNode = this;
                    while (parent != null && !(parent is Composite))
                    {
                        childNode = parent;
                        parent = childNode.ParentNode;
                    }

                    //如果是并行节点


                    if (stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                    {
                        if (isObserving)
                        {
                            isObserving = false;
                            StopObserving();
                        }
                    }

                    ((Composite)parent).StopLowerPriorityChildrenForChild(childNode, stopPolicy == E_StopPolicy.IMMEDIATE_RESTART);
                }
            }
        }

        protected override void StartObserving()
        {
            Root.Clock.AddTimer(interval, -1, Evaluate);
        }

        protected override void StopObserving()
        {
            Root.Clock.RemoveTimer(Evaluate);
        }

        public override void Clear()
        {
            base.Clear();
            interval = 0f;
            eventName = null;
        }
    }
}

