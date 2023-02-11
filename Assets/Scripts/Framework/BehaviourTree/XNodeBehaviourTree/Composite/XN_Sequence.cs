using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Sequence : XN_Composite
    {
        int currentIndex;
        //public XN_Sequence(params Node[] childs) : base("Sequence", childs)
        //{

        //}
        protected override void DoStart()
        {
            currentIndex = -1;
            ProcessChildNode();
        }

        protected override void DoStop()
        {
            children[currentIndex].Stop();
        }

        protected override void DoChildStopped(XN_NodeBase child, bool result)
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

        public override void StopLowerPriorityChildrenForChild(XN_NodeBase childNode, bool isimmediateRestart)
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
        public override NodeDataBase CreateNodeData(int id)
        {
            return new SequenceData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Composite
            };
        }
        public override Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<BehaviorTree.Sequence>();
        }
    }
}