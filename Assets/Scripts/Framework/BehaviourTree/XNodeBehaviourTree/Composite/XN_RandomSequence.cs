using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_RandomSequence : XN_Composite
    {
        
        int currentIndex;
        //public XN_RandomSequence(params Node[] childs) : base("RandomSequence", childs)
        //{

        //}
        protected override void DoStart()
        {
            currentIndex = -1;
            // œ¥≈∆À„∑®
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
                else if (currentNode != childNode)
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

        public override NodeDataBase CreateNodeData(int id)
        {
            return new RandomSequenceData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Composite
            };
        }

        public override Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<BehaviorTree.RandomSequence>();
        }
    }
}

