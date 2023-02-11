using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.ObjectPool
{
    public partial class ObjectPoolManager : SingletonBase<ObjectPoolManager>
    {
        private Dictionary<TypeNamePair, ObjectPoolBase> poolDic;

        private ObjectPoolManager() 
        {
            poolDic = new Dictionary<TypeNamePair, ObjectPoolBase>();
        }
        public IObjectPool<T> CreateObjectPool<T>(string name) where T : ObjectBase
        {
            return InternalCreateObjectPool<T>(name);
        }

        public ObjectPoolBase CreateObjectPool(string name , Type type)
        {
            return InternalCreateObjectPool(name, type);
        }

        public bool HasObjectPool<T>(string name)
        {
            TypeNamePair typeNamePair = new TypeNamePair(name, typeof(T));
            return InternalHasObjectPool(typeNamePair);
        }

        public IObjectPool<T> GetObjectPool<T>(string name) where T : ObjectBase
        {
            TypeNamePair typeNamePair = new TypeNamePair(name, typeof(T));
            if (InternalHasObjectPool(typeNamePair))
            {
                return poolDic[typeNamePair] as IObjectPool<T>;
            }
            return null;
        }

        public ObjectPoolBase GetObjectPool(string name, Type type)
        {
            TypeNamePair typeNamePair = new TypeNamePair(name, type);
            if (InternalHasObjectPool(typeNamePair))
            {
                return poolDic[typeNamePair];
            }
            return null;
        }

        public void Release()
        {
            foreach (var objectPool in poolDic.Values)
            {
                objectPool.Release();
            }
            poolDic.Clear();
        }
        private IObjectPool<T> InternalCreateObjectPool<T>(string name) where T : ObjectBase
        {
            TypeNamePair typeNamePair = new TypeNamePair(name, typeof(T));
            if (InternalHasObjectPool(typeNamePair))
            {
                throw new Exception($"Already exist object pool {name}.");
            }
            ObjectPool<T> objectPool = new ObjectPool<T>(name);
            poolDic.Add(typeNamePair, objectPool);
            return objectPool;
        }
        private ObjectPoolBase InternalCreateObjectPool(string name,Type type)
        {
            if (type == null)
            {
                throw new Exception("Object type is Invaild");
            }
            if (!typeof(ObjectBase).IsAssignableFrom(type))
            {
                throw new Exception($"Object type {type} is Invaild");
            }
            TypeNamePair typeNamePair = new TypeNamePair(name, type);
            if (InternalHasObjectPool(typeNamePair))
            {
                throw new Exception($"Already exist object pool {name}.");
            }

            Type objectPoolType = typeof(ObjectPool<>).MakeGenericType(type);
            ObjectPoolBase objectPool = Activator.CreateInstance(objectPoolType,name) as ObjectPoolBase;
            poolDic.Add(typeNamePair, objectPool);
            return objectPool;
        }

        private bool InternalHasObjectPool(TypeNamePair typeNamePair)
        {
            return poolDic.ContainsKey(typeNamePair);
        }
    }
}

