using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public partial class ObjectPoolManager
    {
        private sealed class ObjectPool<T> : ObjectPoolBase, IObjectPool<T> where T : ObjectBase
        {
            private Dictionary<string, Queue<Object<T>>> objectDic;
            private Dictionary<object, Object<T>> objectMap;
            public override Type ObjectType => typeof(T);

            public override int Count => objectMap.Count;

            public override float ExpireTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

            public ObjectPool(string name):base(name)
            {
                this.objectDic = new Dictionary<string, Queue<Object<T>>>();
                this.objectMap = new Dictionary<object, Object<T>>();
            }


            /// <summary>
            /// 注册对象
            /// </summary>
            /// <param name="obj">注册的对象</param>
            /// <param name="spawned">对象是否已被获取</param>
            public void Register(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("Object is Invalid");
                }
                Object<T> internalObject = Object<T>.Create(obj, spawned);
                if (objectDic.TryGetValue(obj.Name,out Queue<Object<T>> objQueue))
                {
                    objQueue.Enqueue(internalObject);
                }
                else
                {
                    objectDic.Add(obj.Name, new Queue<Object<T>>());
                    objectDic[obj.Name].Enqueue(internalObject);
                }
                objectMap.Add(obj.Target, internalObject);

            }

            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns>要获取的对象</returns>
            public T Spawn()
            {
                return Spawn(string.Empty);
            }

            /// <summary>
            /// 获取对象
            /// </summary>
            /// <returns>要获取的对象</returns>
            public T Spawn(string objName)
            {
                if (objName == null)
                {
                    throw new Exception("objName is Invalid");
                }
                if (objectDic.TryGetValue(objName,out Queue<Object<T>> objQueue))
                {
                    foreach (Object<T> obj in objQueue)
                    {
                        if (!obj.IsInUse)
                        {
                            return obj.Spawn();
                        }
                    }
                }

                return null;
            }

            public void UnSpawn(T obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is Invalid");
                }
                UnSpawn(obj.Target);
            }
            public void UnSpawn(object obj)
            {
                if (obj == null)
                {
                    throw new Exception("Object is Invalid");
                }
                if (objectMap.TryGetValue(obj, out Object<T> internalObject))
                {
                    internalObject.UnSpawn();
                }
            }

            public override void Release()
            {
                foreach (KeyValuePair<object, Object<T>> objectInMap in objectMap)
                {
                    objectInMap.Value.Release();
                    ReferencePool.Instance.Release(objectInMap.Value);
                }
                objectDic.Clear();
                objectMap.Clear();
            }

            public override void Update()
            {
                
            }
        }
    }
}

