using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using XNode;

namespace YMFramework.BehaviorTreeEditor
{
    /// <summary>
    /// ��Ͻڵ㣨�ɰ�������ӽڵ㣩
    /// </summary>
    public abstract class XN_Composite : XN_Container
    {
        [Output]
        int output;
        [ListDrawerSettings]
        public List<XN_NodeBase> children = new List<XN_NodeBase>();
        //protected XN_Composite(string name, params XN_NodeBase[] childs) : base(name)
        //{
        //    this.children = new List<XN_NodeBase>(childs);
        //    foreach (var childNode in childs)
        //    {
        //        childNode.SetParent(this);
        //    }
        //}

        public override void SetRoot(XN_Root root)
        {
            base.SetRoot(root);
            foreach (var childNode in children)
            {
                childNode.SetRoot(root);
            }
        }
        protected override void Stopped(bool result)
        {
            base.Stopped(result);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].ParentCompositeStopped(this);
            }
        }

        /// <summary>
        /// ĳЩ�۲�ڵ���������������ֹͣ���ȼ��͵��ӽڵ㣨�������ȼ�������ִ���Ⱥ�˳�򣩣������ݲ����Ƿ����������ӽڵ�
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="isimmediateRestart"></param>
        public abstract void StopLowerPriorityChildrenForChild(XN_NodeBase childNode, bool isimmediateRestart);


        public override void OnCreateConnection(NodePort from, NodePort to)
        {
            if (from.node == this)
            {
                children.Add(to.node as XN_NodeBase);
                foreach (var childNode in children)
                {
                    childNode.SetParent(this);
                }
            }
        }

        public override void OnRemoveConnection(NodePort from, NodePort to)
        {
            if (from.node == this)
            {
                if(from.IsOutput)
                {
                    children.Remove(to.node as XN_NodeBase);
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
                    children.Remove(from.node as XN_NodeBase);
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


