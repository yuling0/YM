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
            PROCESSING,     //当前多帧Action正在执行
            SUCESS,         //执行成功
            FAILED          //执行失败
        }

        [LabelText("行为函数名称")]
        public string eventName;

        public System.Action action = null;      //普通没有返回值的Action
        
        Func<bool> SingleFrameFunc;     //单帧带执行结果的Func

        Func<bool, E_Result> MultipleFrameFunc;      //多帧需要返回E_Result的Func
        // 这里规定：在多帧的Func中传入 false表示默认执行顺序，当传入true时，表示行为树需要停止了（即执行了Stop()）
        // 所以写函数逻辑时，检测到true，需要返回一个执行结果（E_Result.SUCESS 或 E_Result.FALLED）
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
                Debug.Log("未找到该函数");
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
                Debug.Log("函数与委托类型未匹配");
            }
        }
        protected override void DoStart()
        {
            ProcessAction();
        }



        protected override void DoStop()
        {
            //多帧Action才可能调用
            if (MultipleFrameFunc != null)
            {
                E_Result result = MultipleFrameFunc.Invoke(true);

                if(result == E_Result.PROCESSING)
                {
                    Debug.LogError("行为树停止，需要返回一个执行结果");
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
            E_Result result = MultipleFrameFunc.Invoke(false);//传入false 默认正常执行

            if (result != E_Result.PROCESSING)     //还未执行结束
            {
                Stopped(result == E_Result.FAILED ? false : true);
            }
        }

        /// <summary>
        /// 递归执行多帧函数
        /// </summary>
        private void ProcessFunc()
        {
            E_Result result = MultipleFrameFunc.Invoke(false);//传入false 默认正常执行

            if (result == E_Result.PROCESSING)     //还未执行结束
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