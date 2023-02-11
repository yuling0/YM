using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Repeater : Decorator
    {
        public int loopCount = -1;
        private int currentLoop;
        public Repeater() : base("Repeater")
        {
        }

        protected override void DoStart()
        {
            base.DoStart();
            if (loopCount != 0)
            {
                currentLoop = 0;
                decoratee.Start();
            }
            else
            {
                Stopped(true);
            }
        }

        protected override void DoStop()
        {
            base.DoStop();
            Root.Clock.RemoveTimer(RestartDecoratee);
            if (decoratee.IsActive) 
            {
                decoratee.Stop();
            }
            else
            {
                Stopped(false);
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (result)
            {
                if (IsStopRequested || (loopCount > 0 && ++currentLoop >= loopCount))
                {
                    Stopped(true);
                }
                else
                {
                    Root.Clock.AddTimer(0, 0,RestartDecoratee);
                }
            }
            else
            {
                Stopped(false);
            }
        }

        private void RestartDecoratee()
        {
            decoratee.Start();
        }
    }
}