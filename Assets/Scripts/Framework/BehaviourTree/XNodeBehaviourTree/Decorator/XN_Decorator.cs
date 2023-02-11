using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTreeEditor
{

    /// <summary>
    /// װ�νڵ㣨ֻ����һ���ӽڵ㣩
    /// </summary>
    public abstract class XN_Decorator : XN_Container
    {
        [Output(ShowBackingValue.Never, ConnectionType.Override , TypeConstraint.None)]
        int output;

        [ShowInInspector]
        public XN_NodeBase decoratee = null;
        //public XN_Decorator(string name, XN_NodeBase decorator) : base(name)
        //{
        //    this.decorator = decorator;
        //    decorator.SetParent(this);
        //}

        public override void SetRoot(XN_Root root)
        {
            base.SetRoot(root);
            decoratee.SetRoot(root);

        }
        public override void ParentCompositeStopped(XN_Composite composite)
        {
            base.ParentCompositeStopped(composite);     //ִ���Լ���DoParentCompositeStopped

            decoratee.ParentCompositeStopped(composite);    //ִ��װ�νڵ��DoParentCompositeStopped

        }

        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            if (from.node == this)
            {
                decoratee = to.node as XN_NodeBase;
                (to.node as XN_NodeBase).SetParent(this);
            }
        }

        public override void OnRemoveConnection(NodePort from, NodePort to)
        {
            if (from.node == this)
            {
                if (from.IsOutput)
                {
                    decoratee = null;
                }
                else
                {
                    SetParent(null);
                }
            }
            else
            {
                if (to.IsOutput)
                {
                    decoratee = null;
                }
                else
                {
                    SetParent(null);
                }
            }
            Debug.Log($"{from.node.GetType().Name} �� {to.node.GetType().Name}");
        }
    }

}
