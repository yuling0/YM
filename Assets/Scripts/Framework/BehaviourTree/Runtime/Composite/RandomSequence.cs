using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class RandomSequence : Composite
    {
        
        int currentIndex;
        public RandomSequence(params Node[] childs) : base("RandomSequence", childs)
        {

        }

        public RandomSequence() : base("RandomSequence")
        {
        }

        protected override void DoStart()
        {
            currentIndex = -1;
            // 洗牌算法
            for (int i = 0; i < children.Count; i++)
            {
                int swapIndex = Random.Range(i, children.Count);
                children.Swap<Node>(i, swapIndex);
            }

            ProcessChildNode();
        }
        protected override void DoStop()
        {
            children[currentIndex].Stop();
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            if (result)
            {
                ProcessChildNode();
            }
            else
            {
                Stopped(false);
            }
        }

        private void ProcessChildNode()
        {
            if (++currentIndex < children.Count)
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
                Stopped(true);
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node childNode, bool isimmediateRestart)
        {
            int childIndex = 0;
            bool isFound = false;
            foreach (var currentNode in children)
            {
                if (currentNode == childNode)
                {
                    isFound = true;
                }
                else if (!isFound)
                {
                    childIndex++;
                }
                else if (isFound && currentNode.IsActive)
                {
                    if (isimmediateRestart)
                    {
                        currentIndex = childIndex-1;
                    }
                    else
                    {
                        currentIndex = children.Count;
                    }
                    currentNode.Stop();
                    break;
                }
            }
        }


    }
}

