using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

namespace YMFramework.ObjectPool
{
    public interface IObjectPool<T> where T : ObjectBase
    {
        /// <summary>
        /// 注册对象
        /// </summary>
        /// <param name="obj">注册的对象</param>
        /// <param name="spawned">对象是否已被获取</param>
        void Register(T obj, bool spawned);
        /// <summary>
        /// 获取对象
        /// </summary>
        /// <returns>要获取的对象</returns>
        T Spawn();

        /// <summary>
        /// 获取的对象
        /// </summary>
        /// <param objName="objName">对象的名字</param>
        /// <returns>要获取的对象</returns>
        T Spawn(string objName);

        /// <summary>
        /// 回收的对象
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        void UnSpawn(T obj);

        /// <summary>
        /// 回收的对象
        /// </summary>
        /// <param name="obj">要回收的对象</param>
        void UnSpawn(object obj);
    }
}

