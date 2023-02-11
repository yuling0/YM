using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Wait : Task
    {
        public float seconds;
        public Wait(float seconds) : base("Wait")
        {
            this.seconds = seconds;
        }

        public Wait() : base("Wait")
        {
        }

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
            this.Root.Clock.RemoveTimer(OnTime);
            Stopped(true);
        }
        public override void Clear()
        {
            base.Clear();
            seconds = 0f;
        }
    }

}
