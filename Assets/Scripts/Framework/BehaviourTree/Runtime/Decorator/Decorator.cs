using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTree
{

    /// <summary>
    /// 装饰节点（只包含一个子节点）
    /// </summary>
    public abstract class Decorator : Container
    {
        protected Node decoratee = null;
#if UNITY_EDITOR
        public Node CurrentDecorator => decoratee;
#endif
        public Decorator(string name) : base(name)
        {

        }
        public Decorator(string name, Node decorator) : base(name)
        {
            this.decoratee = decorator;
            decorator.SetParent(this);
        }

        public override void OnInit()
        {
            base.OnInit();
            decoratee.InitNode(controller);
            decoratee.SetParent(this);
        }
        public override void SetRoot(Root root)
        {
            base.SetRoot(root);
            decoratee.SetRoot(root);

        }

#if UNITY_EDITOR
        protected override void DoStart()
        {

            decoratee.ResetResult();
        }

        public override void ResetResult()
        {
            base.ResetResult();
            decoratee.ResetResult();
        }

#endif
        public override void ParentCompositeStopped(Composite composite)
        {
            base.ParentCompositeStopped(composite);     //执行自己的DoParentCompositeStopped

            decoratee.ParentCompositeStopped(composite);    //执行装饰节点的DoParentCompositeStopped

        }

        public void SetDecorator(Node decorator)
        {
            this.decoratee = decorator;
        }

        public override void Clear()
        {
            base.Clear();
            decoratee = null;
        }

        public override void Release()
        {
            decoratee.Release();
            base.Release();
        }
    }

}
