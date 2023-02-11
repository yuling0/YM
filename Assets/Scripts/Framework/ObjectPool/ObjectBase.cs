using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public abstract class ObjectBase : IReference
    {
        private string name;
        private object target;
        private DateTime lastUseTime;
        /// <summary>
        /// 对象的名称
        /// </summary>
        public string Name { get => name; }
        /// <summary>
        /// 对象实例
        /// </summary>
        public object Target { get => target; }
        /// <summary>
        /// 上次的使用时间
        /// </summary>
        public DateTime LastUseTime { get => lastUseTime; }

        protected void Init(object target) => Init(string.Empty, target);
        protected void Init(string name, object target)
        {
            this.name = name;
            this.target = target;
        }

        internal virtual void OnSpawn()
        {
            lastUseTime = DateTime.UtcNow;
        }

        internal virtual void OnUnSpawn()
        {

        }

        public virtual void Clear()
        {
            name = null;
            target = null;
            lastUseTime = default;
        }

        public virtual void Release() 
        {
        
        }
    }

}
