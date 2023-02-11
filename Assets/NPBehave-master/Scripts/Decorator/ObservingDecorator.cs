using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

namespace NPBehave
{
    public abstract class ObservingDecorator : Decorator
    {
        protected Stops stopsOnChange;
        private bool isObserving;

        public ObservingDecorator(string name, Stops stopsOnChange, Node decoratee) : base(name, decoratee)
        {
            this.stopsOnChange = stopsOnChange;
            this.isObserving = false;
        }

        protected override void DoStart()
        {
            if (stopsOnChange != Stops.NONE)
            {
                if (!isObserving)
                {
                    isObserving = true;
                    StartObserving();
                }
            }

            if (!IsConditionMet())  //不满足条件停止
            {
                Stopped(false);
            }
            else  //满足条件执行装饰的节点
            {
                Decoratee.Start();
            }
        }

        override protected void DoStop()
        {
            Decoratee.Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            Assert.AreNotEqual(this.CurrentState, State.INACTIVE);
            if (stopsOnChange == Stops.NONE || stopsOnChange == Stops.SELF)
            {
                if (isObserving)
                {
                    isObserving = false;
                    StopObserving();
                }
            }
            Stopped(result);
        }

        override protected void DoParentCompositeStopped(Composite parentComposite)
        {
            if (isObserving)
            {
                isObserving = false;
                StopObserving();
            }
        }

        protected void Evaluate()
        {
            if (IsActive && !IsConditionMet())      //这个节点激活但不满足条件时，将它停止，待下次重新启动
            {
                if (stopsOnChange == Stops.SELF || stopsOnChange == Stops.BOTH || stopsOnChange == Stops.IMMEDIATE_RESTART)
                {
                    // Debug.Log( this.key + " stopped self ");
                    this.Stop();
                }
            }
            else if (!IsActive && IsConditionMet())     //没有激活 并且 满足条件 
            {
                if (stopsOnChange == Stops.LOWER_PRIORITY || stopsOnChange == Stops.BOTH || stopsOnChange == Stops.IMMEDIATE_RESTART || stopsOnChange == Stops.LOWER_PRIORITY_IMMEDIATE_RESTART)
                {
                    // Debug.Log( this.key + " stopped other ");
                    Container parentNode = this.ParentNode;
                    Node childNode = this;
                    while (parentNode != null && !(parentNode is Composite))
                    {
                        childNode = parentNode;
                        parentNode = parentNode.ParentNode;
                    }
                    Assert.IsNotNull(parentNode, "NTBtrStops is only valid when attached to a parent composite");
                    Assert.IsNotNull(childNode);
                    if (parentNode is Parallel)
                    {
                        Assert.IsTrue(stopsOnChange == Stops.IMMEDIATE_RESTART, "On Parallel Nodes all children have the same priority, thus Stops.LOWER_PRIORITY or Stops.BOTH are unsupported in this context!");
                    }

                    if (stopsOnChange == Stops.IMMEDIATE_RESTART || stopsOnChange == Stops.LOWER_PRIORITY_IMMEDIATE_RESTART)
                    {
                        if (isObserving)
                        {
                            isObserving = false;
                            StopObserving();
                        }
                    }

                    ((Composite)parentNode).StopLowerPriorityChildrenForChild(childNode, stopsOnChange == Stops.IMMEDIATE_RESTART || stopsOnChange == Stops.LOWER_PRIORITY_IMMEDIATE_RESTART);
                }
            }
        }

        protected abstract void StartObserving();

        protected abstract void StopObserving();

        /// <summary>
        /// 检查条件是否满足
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsConditionMet();

    }
}