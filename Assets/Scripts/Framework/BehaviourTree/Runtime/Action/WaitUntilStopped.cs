using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class WaitUntilStopped : Task
    {
        public bool result;
        public WaitUntilStopped(bool result = false) : base("WaitUntilStopped")
        {
            this.result = result;
        }

        public WaitUntilStopped() : base("WaitUntilStopped")
        {
        }

        protected override void DoStop()
        {
            Stopped(result);
        }

        public override void Clear()
        {
            base.Clear();
            result = false;
        }
    }
}

