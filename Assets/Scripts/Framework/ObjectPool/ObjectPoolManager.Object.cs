using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public partial class ObjectPoolManager
    {
        private class Object<T> :IReference where T : ObjectBase
        {
            private T objectInstance;
            private bool isInUse;
            public string Name
            {
                get
                {
                    return objectInstance.Name;
                }
            }

            public object Target
            {
                get
                {
                    return objectInstance.Target;
                }
            }

            public DateTime LastUseTime
            {
                get
                {
                    return objectInstance.LastUseTime;
                }
            }

            public bool IsInUse
            {
                get
                {
                    return isInUse;
                }
            }
            public static Object<T> Create(T obj, bool spawned)
            {
                if (obj == null)
                {
                    throw new Exception("无效的对象");
                }
                Object<T> internalObject = ReferencePool.Instance.Acquire<Object<T>>();
                internalObject.objectInstance = obj;
                internalObject.isInUse = spawned;
                if (spawned)
                {
                    obj.OnSpawn();
                }
                return internalObject;
            }
            public T Spawn()
            {
                objectInstance.OnSpawn();
                isInUse= true;
                return objectInstance;
            }

            public void UnSpawn()
            {
                objectInstance.OnUnSpawn();
                isInUse = false;
            }
            public void Clear()
            {
                objectInstance = default;
                isInUse= false;
            }

            public void Release()
            {
                objectInstance.Release();
                ReferencePool.Instance.Release(objectInstance);
            }
        }
    }
}

