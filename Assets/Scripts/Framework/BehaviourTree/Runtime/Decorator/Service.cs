using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YMFramework.BehaviorTree
{

    /// <summary>
    /// 服务节点，其实就是带一个委托的装饰节点
    /// </summary>
    public class Service : Decorator
    {
        public System.Action serviceMethod;
        public float interval;
        public string eventName;

        public Service(float interval, string eventName) : base("Service")
        {
            this.eventName = eventName;
            this.interval = interval;
        }
        public Service(float interval, System.Action serviceMethod, Node decorator) : base("Service", decorator)
        {
            this.serviceMethod = serviceMethod;
            this.interval = interval;
        }

        public Service() : base("Service")
        {
        }

        public override void OnInit()
        {
            base.OnInit();
            serviceMethod = GetDelegate(eventName) as System.Action;
        }
        protected override void DoStart()
        {
            base.DoStart();
            this.Root.Clock.AddTimer(interval, -1, serviceMethod);
            decoratee.Start();
        }

        protected override void DoStop()
        {
            decoratee.Stop();
        }


        protected override void DoChildStopped(Node child, bool result)
        {
            this.Root.Clock.RemoveTimer(serviceMethod);
            Stopped(result);
        }

        public override void Clear()
        {
            base.Clear();
            serviceMethod = null;
            interval = 0f;
            eventName = null;
        }
    }
}