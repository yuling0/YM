using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Inverter : Decorator
    {
        public Inverter() : base("Inverter")
        {

        }
        public Inverter(Node decorator) : base("Inverter", decorator)
        {

        }

        protected override void DoStart()
        {
            base.DoStart();
            decoratee.Start();
        }

        protected override void DoStop()
        {
            decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Stopped(!result);
        }

    }
}