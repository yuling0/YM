using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Sequence : Composite
    {
        int currentIndex;
        public Sequence(params Node[] childs) : base("Sequence", childs)
        {

        }

        public Sequence() : base("Sequence")
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
            if(result)
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
                Stopped(true);
            }
        }

        public override void StopLowerPriorityChildrenForChild(Node childNode, bool isimmediateRestart)
        {
            int childIndex = 0;
            bool isFound = false;
            foreach (var currentNode in children)
            {
                if (currentNode ==  childNode)
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
                        currentIndex = childIndex -1;
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