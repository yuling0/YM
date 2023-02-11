using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using YMFramework.BehaviorTree;
using Node = YMFramework.BehaviorTree.Node;

namespace YMFramework.BehaviorTreeEditor
{
    [System.Serializable]
    public abstract class XN_NodeBase : XNode.Node
    {
        [Input(ShowBackingValue.Never, ConnectionType.Override, TypeConstraint.None)]
        public int input;
        public enum State
        {
            INACTIVE,        //未运行
            ACTIVE,          //正在运行  
            STOP_REQUESTED   //代表节点正在停止的状态
        }

        public enum Result   //返回结果的状态
        {
            None,            //未返回状态
            Sucess,          //返回成功
            Failure          //返回失败
        }

        [EnumPaging,LabelText("当前状态")]
        public State currentState;
        [EnumPaging, LabelText("当前结果的状态")]
        public Result currentResult;

        [LabelText("节点描述"),Multiline(4)]
        public string nodeDescription;

        TreeNodeContainer container;

        protected BehaviorTreeController bt;

        public XN_Container parent;

        public XN_Root root;

        public XN_Root Root => root;

        public XN_Container ParentNode => parent;

        Color32 inactiveColor = new Color32(118, 118, 118, 150);
        Color32 activeColor = new Color32(132, 248, 67, 129);
        Color32 sucessColor = new Color32(49, 121, 2, 199);
        Color32 failureColor = new Color32(248, 86, 53, 199);

        public Color GetColor()
        {
            if (currentState == State.ACTIVE)
            {
                return activeColor;
            }
            else if (currentState == State.INACTIVE)
            {
                if (currentResult == Result.Failure)
                {
                    return failureColor;
                }
                else if (currentResult == Result.Sucess)
                {
                    return sucessColor;
                }
            }
            return inactiveColor;
        }

        //public XN_NodeBase(string name)
        //{
        //    this.nodeName = name;

        //}

        public bool IsStopRequested => currentState == State.STOP_REQUESTED;
        public bool IsInActive => currentState == State.INACTIVE;
        public bool IsActive => currentState == State.ACTIVE;

        public void Start()
        {
            currentState = State.ACTIVE;
            DoStart();
        }

        public void Stop()
        {
            currentState = State.STOP_REQUESTED;
            DoStop();
        }

        public void InitNode()
        {
            container = graph as TreeNodeContainer;
            OnInit();
        }
        /// <summary>
        /// 节点初始化调用
        /// </summary>
        public virtual void OnInit()
        {
            
        }
        public virtual void SetRoot(XN_Root root)
        {
            this.root = root;
        }

        public void SetParent(XN_Container parent)
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

            if(parent != null)
            {
                parent.ChildStopped(this, result);
            }

        }

        public virtual void ParentCompositeStopped(XN_Composite composite)
        {
            DoParentCompositeStopped(composite);
        }

        /// <summary>
        /// 当父级的组合节点停止后，需要执行的函数（例如：停止组合节点下子节点的检测函数 Evaluate）
        /// </summary>
        /// <param name="composite"></param>
        protected virtual void DoParentCompositeStopped(XN_Composite composite)
        {

        }

        protected T GetComponet<T>()
        {

            return container.GetComponent<T>();
        }

        protected T GetComponentInCore<T>() where T :ComponentBase
        {
            return container.GetComponentInCore<T>();
        }

        protected Delegate GetDelegate(string functionName)
        {
            return container.GetDelegate(functionName);
        }
            

        /// <summary>
        /// 创建节点数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual NodeDataBase CreateNodeData(int id)
        {
            return null;
        }
        public virtual Node CreateNode()
        {
            return null;
        }

        public override object GetValue(NodePort port)
        {
            return null;
        }
    }
}

