using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_Action : XN_Task
    {
        public enum E_Result
        {
            PROCESSING,     //��ǰ��֡Action����ִ��
            SUCESS,         //ִ�гɹ�
            FAILED          //ִ��ʧ��
        }

        [LabelText("��Ϊ��������")]
        public string eventName;

        public System.Action action = null;      //��ͨû�з���ֵ��Action
        
        Func<bool> SingleFrameFunc;     //��֡��ִ�н����Func

        Func<bool, E_Result> MultipleFrameFunc;      //��֡��Ҫ����E_Result��Func
        // ����涨���ڶ�֡��Func�д��� false��ʾĬ��ִ��˳�򣬵�����trueʱ����ʾ��Ϊ����Ҫֹͣ�ˣ���ִ����Stop()��
        // ����д�����߼�ʱ����⵽true����Ҫ����һ��ִ�н����E_Result.SUCESS �� E_Result.FALLED��
        //public XN_Action(UnityAction action) : base("Action")
        //{
        //    this.action = action;
        //}

        //public XN_Action(Func<bool> SingleFrameFunc) : base("Action")
        //{
        //    this.SingleFrameFunc = SingleFrameFunc;
        //}
        //public XN_Action(Func<bool, E_Result> MultipleFrameFunc) : base("Action")
        //{
        //    this.MultipleFrameFunc = MultipleFrameFunc;
        //}
        public override void OnInit()
        {
            Delegate d = GetDelegate(eventName);

            if (d == null)
            {
                Debug.Log("δ�ҵ��ú���");
            }
            if (d is UnityAction)
            {
                action = d as System.Action;
            }
            else if (d is Func<bool>)
            {
                SingleFrameFunc = d as Func<bool>;
            }
            else if (d is Func<bool, E_Result>)
            {
                MultipleFrameFunc = d as Func<bool, E_Result>;
            }
            else
            {
                Debug.Log("������ί������δƥ��");
            }
        }
        protected override void DoStart()
        {
            ProcessAction();
        }



        protected override void DoStop()
        {
            //��֡Action�ſ��ܵ���
            if (MultipleFrameFunc != null)
            {
                E_Result result = MultipleFrameFunc.Invoke(true);

                if(result == E_Result.PROCESSING)
                {
                    Debug.LogError("��Ϊ��ֹͣ����Ҫ����һ��ִ�н��");
                }
                this.Root.Clock.RemoveTimer(ProcessFunc);
                this.Root.Clock.RemoveObserver(ProcessFunc2);
                Stopped(result == E_Result.FAILED ? false : true);
            }
        }

        private void ProcessAction()
        {
            if(action != null)
            {
                action.Invoke();
                Stopped(true);
            }
            else if(SingleFrameFunc != null)
            {
                Stopped(SingleFrameFunc());
            }

            else if (MultipleFrameFunc != null)
            {
                ProcessFunc();
                //this.Root.Clock.AddObserver(ProcessFunc2);
            }
        }

        private void ProcessFunc2()
        {
            E_Result result = MultipleFrameFunc.Invoke(false);//����false Ĭ������ִ��

            if (result != E_Result.PROCESSING)     //��δִ�н���
            {
                Stopped(result == E_Result.FAILED ? false : true);
            }
        }

        /// <summary>
        /// �ݹ�ִ�ж�֡����
        /// </summary>
        private void ProcessFunc()
        {
            E_Result result = MultipleFrameFunc.Invoke(false);//����false Ĭ������ִ��

            if (result == E_Result.PROCESSING)     //��δִ�н���
            {
                this.Root.Clock.AddTimer(0, 0, ProcessFunc);
            }
            else
            {
                Stopped(result == E_Result.FAILED ? false : true);
            }
        }
        public override Node CreateNode()
        {
            BehaviorTree.Action action = ReferencePool.Instance.Acquire<BehaviorTree.Action>();
            action.eventName = eventName;
            action.action = this.action;
            return action;
        }
        public override NodeDataBase CreateNodeData(int id)
        {
            return new ActionData() { id = id, NodeType = NodeDataBase.E_NodeType.Task, eventName = eventName };
        }
    }
}