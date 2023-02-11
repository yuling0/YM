using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{

    /// <summary>
    /// 服务节点，其实就是带一个委托的装饰节点
    /// </summary>
    public class XN_Service : XN_Decorator
    {
        public System.Action serviceMethod;

        [LabelText("函数执行间隔")]
        public float interval;
        [LabelText("事件函数名")]
        public string eventName;
        //public XN_Service(float interval, UnityAction serviceMethod, XN_NodeBase decorator) : base("Service", decorator)
        //{
        //    this.serviceMethod = serviceMethod;
        //    this.interval = interval;
        //}
        //protected override void DoStart()
        //{
        //    this.Root.Clock.AddTimer(interval, -1, serviceMethod);
        //    decorator.Start();
        //}

        //protected override void DoStop()
        //{
        //    decorator.Stop();
        //}


        //protected override void DoChildStopped(XN_NodeBase child, bool result)
        //{
        //    this.Root.Clock.RemoveTimer(serviceMethod);
        //    Stopped(result);
        //}

        public override NodeDataBase CreateNodeData(int id)
        {
            return new ServiceData()
            {
                id = id,
                NodeType = NodeDataBase.E_NodeType.Decorator,
                checkInterval = interval,
                eventName = eventName
            };
        }

        public override Node CreateNode()
        {
            Service service = ReferencePool.Instance.Acquire<Service>();
            service.interval = interval;
            service.eventName = eventName;
            service.serviceMethod = serviceMethod;
            return service;
        }
    }
}