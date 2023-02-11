using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using XNode;

namespace YMFramework.BehaviorTree
{
    /// <summary>
    /// 组合节点（可包含多个子节点）
    /// </summary>
    public abstract class Composite : Container
    {
        protected List<Node> children = new List<Node>();
#if UNITY_EDITOR
        public List<Node> Children => children;
#endif
        protected Composite(string name, params Node[] childs) : base(name)
        {
            if(childs != null)
            this.children = new List<Node>(childs);
            foreach (var childNode in childs)
            {
                childNode.SetParent(this);
            }
        }

        protected Composite(string name) : base(name)
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            foreach (var childNode in children)
            {
                childNode.InitNode(controller);
                childNode.SetParent(this);
            }
        }
        public override void SetRoot(Root root)
        {
            base.SetRoot(root);
            foreach (var childNode in children)
            {
                childNode.SetRoot(root);
            }
        }
#if UNITY_EDITOR
        protected override void DoStart()
        {

            foreach (var childNode in children)
            {
                childNode.ResetResult();
            }
        }
        public override void ResetResult()
        {
            base.ResetResult();
            foreach (var childNode in children)
            {
                childNode.ResetResult();
            }
        }
#endif

        protected override void Stopped(bool result)
        {
            base.Stopped(result);
            for (int i = 0; i < children.Count; i++)
            {
                children[i].ParentCompositeStopped(this);
            }
        }
        public void AddChild(Node child)
        {
            children.Add(child);
        }

        /// <summary>
        /// 某些观察节点满足条件，尝试停止优先级低的子节点（这里优先级体现在执行先后顺序），并根据策略是否重新启动子节点
        /// </summary>
        /// <param name="childNode"></param>
        /// <param name="isimmediateRestart"></param>
        public abstract void StopLowerPriorityChildrenForChild(Node childNode, bool isimmediateRestart);


        public override void Clear()
        {
            base.Clear();
            children.Clear();
        }

        public override void Release()
        {
            foreach (var childNode in children)
            {
                childNode.Release();
            }
            base.Release();
        }
    }
}


