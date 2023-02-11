using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public partial class ReferencePool
{
    private class ReferenceCollection
    {
        Queue<IReference> references;
        Type referenceType;
        public ReferenceCollection(Type type)
        {
            references = new Queue<IReference>();
            referenceType = type;
        }
        public T Acquire<T>() where T : class, IReference, new()
        {
            if (references.Count > 0)
            {
                return references.Dequeue() as T;
            }
            else
            {
                return new T();
            }
        }

        public void Release(IReference reference)
        {

            if (references.Contains(reference))
            {
                Debug.LogError("对应类型对象：" + referenceType.Name + "已经被释放");
                return;
            }
            reference.Clear();
            references.Enqueue(reference);
            
        }

        public void ClearAll()
        {
            references.Clear();
        }
    }
}

