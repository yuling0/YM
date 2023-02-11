using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public abstract class Node : IReference
    {
        //public enum State    //当前节点的状态
        //{
        //    INACTIVE,        //未运行
        //    ACTIVE,          //正在运行
        //    STOP_REQUESTED   //代表节点正在停止的状态
        //}

#if UNITY_EDITOR
        public enum Result   //返回结果的状态
        {
            None,            //未返回状态
            Sucess,          //返回成功
            Failure          //返回失败
        }

        protected Result currentResult;
        public Result CurrentResult => currentResult;
        public virtual void ResetResult()
        {
            this.currentResult = Result.None;
        }

#endif
        protected State currentState;
        protected string nodeDescription;

        protected string nodeName;

        private Container parent;

        private Root root;
        public Root Root => root;

        public Container ParentNode => parent;

        public State CurrentState => currentState;
        protected BehaviorTreeController controller;


        public Node(string name)
        {
            this.nodeName = name;

        }

        public bool IsStopRequested => currentState == State.STOP_REQUESTED;
        public bool IsInActive => currentState == State.INACTIVE;
        public bool IsActive => currentState == State.ACTIVE;

        public void Start()
        {
            currentState = State.ACTIVE;
#if UNITY_EDITOR
            currentResult = Result.None;
#endif
            DoStart();
        }

        public void Stop()
        {
            currentState = State.STOP_REQUESTED;
            DoStop();
        }

        public void InitNode(BehaviorTreeController controller)
        {
            this.controller = controller;
            OnInit();
        }
        /// <summary>
        /// 节点初始化调用
        /// </summary>
        public virtual void OnInit()
        {
            
        }
        public virtual void SetRoot(Root root)
        {
            this.root = root;
        }

        public void SetParent(Container parent)
        {
            this.parent = parent;
        }

        protected virtual void DoStart()
        {
            
        }

        protected virtual void DoStop()
        {

        }


        protected virtual void Stopped(bool result)
        {
            currentState = State.INACTIVE;
#if UNITY_EDITOR

            currentResult = result ? Result.Sucess : Result.Failure;
#endif
            if(parent != null)
            {
                parent.ChildStopped(this, result);
            }

        }

        public virtual void ParentCompositeStopped(Composite composite)
        {
            DoParentCompositeStopped(composite);
        }

        /// <summary>
        /// 当父级的组合节点停止后，需要执行的函数（例如：停止组合节点下子节点的检测函数 Evaluate）
        /// </summary>
        /// <param name="composite"></param>
        protected virtual void DoParentCompositeStopped(Composite composite)
        {

        }

        protected T GetComponet<T>()
        {

            return controller.GetComponent<T>();
        }

        protected T GetComponentInCore<T>() where T : ComponentBase
        {
            return controller.GetComponentInCore<T>();
        }

        protected Delegate GetDelegate(string functionName)
        {
            return controller.GetDelegate(functionName);
        }

        public override string ToString()
        {
            return nodeName;
        }

        /// <summary>
        /// 恢复默认状态方法（用于引用池复用）
        /// </summary>
        public virtual void Clear()
        {
            controller = null;
            root = null;
            parent = null;
            currentState = State.INACTIVE;
        }

        /// <summary>
        /// 节点的释放（放回引用池）从叶子节点开始释放
        /// </summary>
        public virtual void Release()
        {
            ReferencePool.Instance.Release(this);
        }
    }
}

