using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Selector : XN_Composite
    {
        int currentIndex;
        //public XN_Selector(params Node[] childs) : base("Select", childs)
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
            if(!result)     //�ӽڵ�ִ��ʧ�ܣ�false��
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

        public override void StopLowerPriorityChildrenForChild(XN_NodeBase childNode, bool isimmediateRestart)
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
                        currentIndex = children.Count;   //ֹͣ���ڵ������
                    }
                    else
                    {
                        currentIndex = childIndex -1;      //����ΪĿ���ӽڵ����������������
                    }
                    currentChild.Stop();
                    break;
                }

            }
        }
        public override NodeDataBase CreateNodeData(int id)
        {
            return new SelectorData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Composite
            };
        }
        public override Node CreateNode()
        {
            return ReferencePool.Instance.Acquire<BehaviorTree.Selector>();
        }
    }
}