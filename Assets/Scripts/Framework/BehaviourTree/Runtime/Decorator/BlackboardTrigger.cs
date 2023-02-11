using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class BlackboardTrigger : ObservingDecorator
    {
        public string key;
        public BlackboardTrigger() : base("BlackboardTrigger")
        {
        }

        public BlackboardTrigger(string key,E_StopPolicy stopPolicy) : base("BlackboardTrigger", stopPolicy)
        {
            this.key = key;
        }

        public BlackboardTrigger(string key, E_StopPolicy stopPolicy, Node decorator) : base("BlackboardTrigger", stopPolicy, decorator)
        {
            this.key = key;
        }
        protected override void Evaluate()
        {
            if (!IsActive && IsConditionMet())
            {
                if (stopPolicy == E_StopPolicy.AbortParent || stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                {
                    Node cur = this;
                    Node parent = this.ParentNode;
                    while (parent != null && !(parent is Composite))
                    {
                        cur = parent;
                        parent = cur.ParentNode;
                    }

                    if (stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                    {
                        if (isObserving)
                        {
                            isObserving = false;
                            StopObserving();
                        }
                    }

                    ((Composite)parent).StopLowerPriorityChildrenForChild(cur, stopPolicy == E_StopPolicy.IMMEDIATE_RESTART);

                }


            }
        }
        protected override bool IsConditionMet()
        {
            return Root.Blackboard.IsTrigger(key);
        }

        protected override void StartObserving()
        {
            Root.Blackboard.AddObserver(key,OnChanged);
        }


        protected override void StopObserving()
        {
            Root.Blackboard.RemoveObserver(key, OnChanged);
        }

        private void OnChanged(E_ChangeType arg1, ISharedType arg2)
        {
            Evaluate();
        }

    }

}
