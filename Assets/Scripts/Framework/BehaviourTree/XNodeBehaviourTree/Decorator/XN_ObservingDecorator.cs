using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public abstract class XN_ObservingDecorator : XN_Decorator
    {
        //public enum E_StopPolicy
        //{
        //    /// <summary>
        //    /// 默认策略：当子节点不满足条件时停止观察条件
        //    /// </summary>
        //    NONE,
        //    /// <summary>
        //    /// 终止父节点：即使在不满足条件下也会观察条件是否满足，当此节点未激活并且满足条件时，将会终止父节点的后续执行
        //    /// </summary>
        //    AbortParent,
        //    /// <summary>
        //    /// 立刻重启：当此节点未激活并且满足条件时，会停止父节点当前正在执行的节点，并立刻启动此节点
        //    /// </summary>
        //    IMMEDIATE_RESTART
        //}
        public bool isObserving = false;
        [EnumPaging,LabelText("停止策略")]
        public E_StopPolicy stopPolicy;
        //public XN_ObservingDecorator(string name, E_StopPolicy stopPolicy, XN_NodeBase decorator) : base(name, decorator)
        //{
        //    this.stopPolicy = stopPolicy;
        //}

        protected override void DoStart()
        {
            if (!isObserving && stopPolicy != E_StopPolicy.NONE)
            {
                isObserving = true;
                StartObserving();
            }
            if (IsConditionMet())   //满足条件执行节点
            {
                decoratee.Start();
            }
            else
            {
                Stopped(false);
            }

            Debug.Log($"哦呦呦{stopPolicy}");
        }

        
        protected override void DoStop()
        {
            
            decoratee.Stop();

        }

        protected override void DoChildStopped(XN_NodeBase child, bool result)
        {
            if (stopPolicy == E_StopPolicy.NONE)
            {
                if (isObserving)
                {
                    isObserving = false;
                    StopObserving();
                }
            }
            Stopped(result);
        }

        protected override void DoParentCompositeStopped(XN_Composite composite)
        {
            if (isObserving)
            {
                isObserving = false;
                StopObserving();
            }
        }
        protected void Evaluate()
        {
            if (IsActive && !IsConditionMet()) //当此节点启用 并且 条件不满足时停止此节点执行
            {
                this.Stop();
            }
            else if (!IsActive && IsConditionMet()) //此节点未启用，并且当前条件满足
            {
                if (stopPolicy == E_StopPolicy.AbortParent || stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                {
                    XN_Container parent = this.ParentNode;
                    XN_NodeBase childNode = this;
                    while (parent != null && !(parent is XN_Composite))
                    {
                        childNode = parent;
                        parent = childNode.ParentNode;
                    }

                    //如果是并行节点


                    if (stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                    {
                        if (isObserving)
                        {
                            isObserving = false;
                            StopObserving();
                        }
                    }

                    ((XN_Composite)parent).StopLowerPriorityChildrenForChild(childNode, stopPolicy == E_StopPolicy.IMMEDIATE_RESTART);
                }
            }
        }
        protected abstract void StartObserving();

        protected abstract void StopObserving();

        protected abstract bool IsConditionMet();

    }
}