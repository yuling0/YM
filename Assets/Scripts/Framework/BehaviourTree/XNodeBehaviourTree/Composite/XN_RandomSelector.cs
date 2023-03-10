using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_RandomSelector : XN_Composite
    {
        int currentIndex;
        //public XN_RandomSelector(params XN_NodeBase[] childs) : base("RandomSelector", childs)
        //{

        //}
        protected override void DoStart()
        {
            currentIndex = -1;

            //ϴ???㷨
            for (int i = 0; i < children.Count; i++)
            {
                int swapIndex = Random.Range(i, children.Count);

                children.Swap<XN_NodeBase>(i, swapIndex);

            }

            ProcessChildNode();

        }

        protected override void DoStop()
        {
            children[currentIndex].Stop();
        }

        protected override void DoChildStopped(XN_NodeBase child, bool result)
        {
            if (!result)
            {
                ProcessChildNode();
            }
            else
            {
                Stopped(result);
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
                Stopped(false);
            }
        }

        public override void StopLowerPriorityChildrenForChild(XN_NodeBase childNode, bool isimmediateRestart)
        {
            int childIndex = 0;
            bool isFound = false;
            foreach (var currentNode in children)
            {
                if (currentNode == childNode)
                {
                    isFound = true;
                }
                else if (childNode != currentNode)
                {
                    childIndex++;
                }
                else if (isFound && currentNode.IsActive)
                {
                    if (isimmediateRestart)
                    {
                        currentIndex = childIndex - 1;
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
        public override NodeDataBase CreateNodeData(int id)
        {
            return new RandomSelectorData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Composite
            };
        }
        public override Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<BehaviorTree.RandomSelector>();
        }
    }
}

