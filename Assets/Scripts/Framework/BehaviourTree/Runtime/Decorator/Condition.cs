using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Condition : ObservingDecorator
    {

        public Func<bool> condition;

        public float checkInterval;

        public string eventName;

        public Condition(E_StopPolicy stopPolicy, string eventName, float checkInterval) : base("Condition", stopPolicy)
        {
            this.eventName = eventName;
            this.checkInterval = checkInterval;
        }
        public Condition(E_StopPolicy stopPolicy, Func<bool> condition, float checkInterval, Node decorator) : base("Condition", stopPolicy, decorator)
        {
            this.condition = condition;
            this.checkInterval = checkInterval;
        }
        public Condition():base("Condition")
        {

        }

        public override void OnInit()
        {
            base.OnInit();
            condition = GetDelegate(eventName) as Func<bool>;
        }
        protected override bool IsConditionMet()
        {
            return condition.Invoke();
        }

        protected override void StartObserving()
        {

            this.Root.Clock.AddTimer(checkInterval, -1, Evaluate);
        }

        protected override void StopObserving()
        {
            this.Root.Clock.RemoveTimer(Evaluate);
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            base.DoChildStopped(child, result);
        }

        public override void Clear()
        {
            base.Clear();
            condition = null;
            checkInterval = 0f;
            eventName = null;
        }
    }
}