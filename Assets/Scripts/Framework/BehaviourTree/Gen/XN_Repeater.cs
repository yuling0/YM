using Sirenix.OdinInspector;
using System;
using UnityEngine;
using YMFramework.BehaviorTree;
namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Repeater : XN_Decorator
    {
		public System.Int32 loopCount;

        public override Node CreateNode()
        {
            Repeater node= ReferencePool.Instance.Acquire<Repeater>();
			node.loopCount = loopCount;

            return node;
        }
    }
}