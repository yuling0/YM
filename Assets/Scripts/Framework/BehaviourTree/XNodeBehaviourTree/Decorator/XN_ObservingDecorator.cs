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
        //    /// Ĭ�ϲ��ԣ����ӽڵ㲻��������ʱֹͣ�۲�����
        //    /// </summary>
        //    NONE,
        //    /// <summary>
        //    /// ��ֹ���ڵ㣺��ʹ�ڲ�����������Ҳ��۲������Ƿ����㣬���˽ڵ�δ�������������ʱ��������ֹ���ڵ�ĺ���ִ��
        //    /// </summary>
        //    AbortParent,
        //    /// <summary>
        //    /// �������������˽ڵ�δ�������������ʱ����ֹͣ���ڵ㵱ǰ����ִ�еĽڵ㣬�����������˽ڵ�
        //    /// </summary>
        //    IMMEDIATE_RESTART
        //}
        public bool isObserving = false;
        [EnumPaging,LabelText("ֹͣ����")]
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
            if (IsConditionMet())   //��������ִ�нڵ�
            {
                decoratee.Start();
            }
            else
            {
                Stopped(false);
            }

            Debug.Log($"Ŷ����{stopPolicy}");
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
            if (IsActive && !IsConditionMet()) //���˽ڵ����� ���� ����������ʱֹͣ�˽ڵ�ִ��
            {
                this.Stop();
            }
            else if (!IsActive && IsConditionMet()) //�˽ڵ�δ���ã����ҵ�ǰ��������
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

                    //����ǲ��нڵ�


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