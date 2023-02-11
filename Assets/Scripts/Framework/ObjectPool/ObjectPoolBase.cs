using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public abstract class ObjectPoolBase
    {
        private readonly string objectPoolName;

        public ObjectPoolBase() : this(null)
        {

        }

        public ObjectPoolBase(string objectPoolName)
        {
            this.objectPoolName = objectPoolName;
        }

        /// <summary>
        /// 获取对象池的名字
        /// </summary>
        public string Name
        {
            get 
            {
                return objectPoolName; 
            }
        }
        /// <summary>
        /// 获取对象池中对象类型
        /// </summary>
        public abstract Type ObjectType
        {
            get;
        }
        /// <summary>
        /// 获取对象池中数量
        /// </summary>
        public abstract int Count
        {
            get;
        }

        public abstract float ExpireTime
        {
            get;
            set;
        }
        /// <summary>
        /// 释放对象池中的对象
        /// </summary>
        public abstract void Release();

        public abstract void Update();
    }

}

