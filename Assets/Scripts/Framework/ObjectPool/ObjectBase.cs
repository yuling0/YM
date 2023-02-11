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
        /// ���������
        /// </summary>
        public string Name { get => name; }
        /// <summary>
        /// ����ʵ��
        /// </summary>
        public object Target { get => target; }
        /// <summary>
        /// �ϴε�ʹ��ʱ��
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
