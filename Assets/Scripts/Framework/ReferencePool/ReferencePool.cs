using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 引用池(装一些C#的对象，不是GameObject类型的)
/// </summary>
public partial class ReferencePool : SingletonBase<ReferencePool>
{
    private Dictionary<Type, ReferenceCollection> referenceCollections;

    private ReferencePool()
    {
        referenceCollections = new Dictionary<Type, ReferenceCollection>();
    }


    public T Acquire<T>() where T : class, IReference, new()
    {
        Type type = typeof(T);
        if(!referenceCollections.ContainsKey(type))
        {
            referenceCollections.Add(type, new ReferenceCollection(type));
        }
        return referenceCollections[type].Acquire<T>();
    }


    public void Release(IReference reference)
    {
        Type type = reference.GetType();

        if (!referenceCollections.ContainsKey(type))
        {
            referenceCollections.Add(type, new ReferenceCollection(type));
        }

        referenceCollections[type].Release(reference);
    }

    public void ClearAll()
    {
        foreach (var references in referenceCollections.Values)
        {
            references.ClearAll();
        }

        referenceCollections.Clear();
    }
}
