using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTreeEditor
{
    /// <summary>
    /// 容器节点，一般不作为叶子节点
    /// </summary>
    public abstract class XN_Container : XN_NodeBase 
    {

        //public XN_Container(string name) : base(name)
        //{

        //}

        /// <summary>
        /// 子节点停止时，执行父节点的该方法
        /// </summary>
        /// <param name="child">子节点</param>
        /// <param name="result">子节点的执行结果</param>
        public void ChildStopped(XN_NodeBase child, bool result)
        {
            DoChildStopped(child,result);
        }

        /// <summary>
        /// 父节点根据子节点的执行结果，来决定自己的执行策略
        /// </summary>
        /// <param name="child">子节点</param>
        /// <param name="result">子节点的执行结果</param>
        protected virtual void DoChildStopped(XN_NodeBase child,bool result)
        {

        }
    }

}
