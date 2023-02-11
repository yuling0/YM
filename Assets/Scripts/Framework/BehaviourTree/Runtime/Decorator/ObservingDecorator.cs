using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YMFramework.BehaviorTree
{
    public abstract class ObservingDecorator : Decorator
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
        protected bool isObserving = false;


        public E_StopPolicy stopPolicy;

        public ObservingDecorator(string name, E_StopPolicy stopPolicy) : base(name)
        {
            this.stopPolicy = stopPolicy;
        }
        public ObservingDecorator(string name, E_StopPolicy stopPolicy, Node decorator) : base(name, decorator)
        {
            this.stopPolicy = stopPolicy;
        }


        protected ObservingDecorator(string name) :base(name)
        {

        }

        protected override void DoStart()
        {
            base.DoStart();
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

        }

        
        protected override void DoStop()
        {
            decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
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

        protected override void DoParentCompositeStopped(Composite composite)
        {
            if (isObserving)
            {
                isObserving = false;
                StopObserving();
            }
        }
        protected virtual void Evaluate()
        {
            if (IsActive && !IsConditionMet()) //当此节点启用 并且 条件不满足时停止此节点执行
            {
                this.Stop();
            }
            else if (!IsActive && IsConditionMet()) //此节点未启用，并且当前条件满足
            {
                if (stopPolicy == E_StopPolicy.AbortParent || stopPolicy == E_StopPolicy.IMMEDIATE_RESTART)
                {
                    Container parent = this.ParentNode;
                    Node childNode = this;
                    while (parent != null && !(parent is Composite))
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

                    ((Composite)parent).StopLowerPriorityChildrenForChild(childNode, stopPolicy == E_StopPolicy.IMMEDIATE_RESTART);
                }
            }
        }
        protected abstract void StartObserving();

        protected abstract void StopObserving();

        protected abstract bool IsConditionMet();


        public override void Clear()
        {
            base.Clear();
            isObserving = false;
            stopPolicy = E_StopPolicy.NONE;
        }
    }
}