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
        /// ��ȡ����ص�����
        /// </summary>
        public string Name
        {
            get 
            {
                return objectPoolName; 
            }
        }
        /// <summary>
        /// ��ȡ������ж�������
        /// </summary>
        public abstract Type ObjectType
        {
            get;
        }
        /// <summary>
        /// ��ȡ�����������
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
        /// �ͷŶ�����еĶ���
        /// </summary>
        public abstract void Release();

        public abstract void Update();
    }

}

