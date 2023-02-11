using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Selector : Composite
    {
        int currentIndex;
        public Selector(params Node[] childs) : base("Select", childs)
        {

        }

        public Selector() : base("Select")
        {
        }

        protected override void DoStart()
        {
            base.DoStart();
            currentIndex = -1;
            ProcessChildNode();
        }

        protected override void DoStop()
        {
            children[currentIndex].Stop();
        }
        protected override void DoChildStopped(Node child, bool result)
        {
            if(!result)     //子节点执行失败（false）
            {
                ProcessChildNode();
            }
            else
            {
                Stopped(true);
            }

        }
        private void ProcessChildNode()
        {
            if(++currentIndex < children.Count)
            {
                if (IsStopRequested)
                {
                    Stopped(false);
                }
                else
                {
                    children[currentIndex].Start();
                }
                
            }
            else
            {
                Stopped(false);
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node childNode, bool isimmediateRestart)
        {
            int childIndex = 0;
            bool isFound = false;
            foreach (var currentChild in children)
            {
                if (currentChild == childNode)
                {
                    isFound = true;
                }
                else if (!isFound)
                {
                    childIndex++;
                }
                else if(isFound && currentChild.IsActive)
                {
                    if (!isimmediateRestart)
                    {
                        currentIndex = children.Count;   //停止父节点的运行
                    }
                    else
                    {
                        currentIndex = childIndex -1;      //设置为目标子节点的索引，立即重启
                    }
                    currentChild.Stop();
                    break;
                }

            }
        }
    }
}