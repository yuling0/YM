using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

namespace YMFramework.ObjectPool
{
    public interface IObjectPool<T> where T : ObjectBase
    {
        /// <summary>
        /// ע�����
        /// </summary>
        /// <param name="obj">ע��Ķ���</param>
        /// <param name="spawned">�����Ƿ��ѱ���ȡ</param>
        void Register(T obj, bool spawned);
        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        T Spawn();

        /// <summary>
        /// ��ȡ�Ķ���
        /// </summary>
        /// <param objName="objName">���������</param>
        /// <returns>Ҫ��ȡ�Ķ���</returns>
        T Spawn(string objName);

        /// <summary>
        /// ���յĶ���
        /// </summary>
        /// <param name="obj">Ҫ���յĶ���</param>
        void UnSpawn(T obj);

        /// <summary>
        /// ���յĶ���
        /// </summary>
        /// <param name="obj">Ҫ���յĶ���</param>
        void UnSpawn(object obj);
    }
}

