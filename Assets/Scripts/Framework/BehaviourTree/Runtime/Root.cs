using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTree
{
    public class Root : Decorator
    {
        GlobalClock clock;
        Blackboard blackboard;
        public GlobalClock Clock => clock;

        public Blackboard Blackboard => blackboard;

        public Root() : base("Root")
        {

        }
        public Root(Node decorator) : base("Root", decorator)
        {
            clock = GlobalClock.Instance;
            blackboard = new Blackboard(clock);
            SetRoot(this);
        }
        public override void OnInit()
        {
            base.OnInit();
            clock = GlobalClock.Instance;
            blackboard = new Blackboard(clock);
            SetRoot(this);
        }
        //public override void SetRoot(Root root)
        //{
        //    decorator.SetRoot(this);
        //}

        protected override void DoStart()
        {
            base.DoStart();
            decoratee.Start();
        }

        protected override void DoStop()
        {
            if (this.decoratee.IsActive)
            {
                decoratee.Stop();
            }
            else
            {
                this.clock.RemoveTimer(decoratee.Start);
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if(!IsStopRequested)
            {
                clock.AddTimer(0, 0, decoratee.Start);
            }
            else
            {
                Stopped(result);
            }
        }

    }
}

