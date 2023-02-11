using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface PoolObjBase
{
    public void ClearPool();
}
/// <summary>
/// ≥ÿ∂‘œÛ
/// </summary>
/// <typeparam name="T"></typeparam>
public class PoolObj<T> : PoolObjBase where T : Object
{
    private Queue<T> poolList;
    private Transform poolRoot;
    private string resPath;
    public PoolObj(Transform poolMgr , string path)
    {
        poolList = new Queue<T>();
        GameObject obj = new GameObject();
        obj.name = path;
        poolRoot = obj.transform;
        poolRoot.SetParent(poolMgr);
        resPath = path;
    }

    public void PushObj(T obj)
    {
        poolList.Enqueue(obj);
        if(obj is GameObject)
        {
            (obj as GameObject).transform.SetParent(poolRoot);
            (obj as GameObject).SetActive(false);
        }
    }

    public T PopObj()
    {
        if (poolList.Count > 0)
        {
            T obj = poolList.Dequeue();
            if (obj is GameObject)
            {
                (obj as GameObject).transform.SetParent(null);
                (obj as GameObject).SetActive(true);
            }
            return obj;
        }

        return ResourceMgr.Instance.LoadAsset<T>(resPath);
    }

    public void PopObjAsync(UnityAction<T> callback)
    {
        if (poolList.Count > 0)
        {
            T obj = poolList.Dequeue();
            if(obj is GameObject)
            {
                (obj as GameObject).transform.SetParent(null);
                (obj as GameObject).SetActive(true);
            }
            callback?.Invoke(obj);
        }
        else
        {
            ResourceMgr.Instance.LoadAssetAsync<T>(resPath, callback);
        }

    }

    public void ClearPool()
    {
        poolList.Clear();
        GameObject.Destroy(poolRoot);
    }
}
