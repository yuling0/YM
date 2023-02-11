using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace YMFramework.BehaviorTree
{
    public class Action : Task
    {
        //public enum E_Result
        //{
        //    PROCESSING,     //当前多帧Action正在执行
        //    SUCESS,         //执行成功
        //    FAILED          //执行失败
        //}

        public string eventName;

        public System.Action action = null;      //普通没有返回值的Action

        Func<bool> SingleFrameFunc;     //单帧带执行结果的Func

        Func<bool, E_Result> MultipleFrameFunc;      //多帧需要返回E_Result的Func
        // 这里规定：在多帧的Func中传入 false表示默认执行顺序，当传入true时，表示行为树需要停止了（即执行了Stop()）
        // 所以写函数逻辑时，检测到true，需要返回一个执行结果（E_Result.SUCESS 或 E_Result.FALLED）
        public Action(System.Action action) : base("Action")
        {
            this.action = action;
        }

        public Action(Func<bool> SingleFrameFunc) : base("Action")
        {
            this.SingleFrameFunc = SingleFrameFunc;
        }
        public Action(Func<bool, E_Result> MultipleFrameFunc) : base("Action")
        {
            this.MultipleFrameFunc = MultipleFrameFunc;
        }

        public Action(string eventName) : base("Action")
        {
            this.eventName = eventName;

        }

        public Action() : base("Action")
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            Delegate del = GetDelegate(eventName);
            if (del is System.Action)
            {
                action = del as System.Action;
            }
            else if (del is Func<bool>)
            {
                SingleFrameFunc = del as Func<bool>;
            }
            else if (del is Func<bool, E_Result>)
            {
                MultipleFrameFunc = del as Func<bool, E_Result>;
            }
            else
            {
                Debug.Log($"未能绑定函数名为：{eventName}的函数委托");
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
            else
            {
                Debug.Log($"该Action节点未执行任何函数委托,函数委托名为：{eventName}");
                Stopped(true);
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

        public override void Clear()
        {
            base.Clear();
            eventName = null;
            action = null;
            SingleFrameFunc = null;
            MultipleFrameFunc = null;
        }
    }
}