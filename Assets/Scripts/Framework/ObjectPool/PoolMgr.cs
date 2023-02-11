using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// 对象池管理类
/// </summary>
public class PoolMgr : SingletonBase<PoolMgr>
{
    //对象池容器
    private Dictionary<string, PoolObjBase> pools = new Dictionary<string, PoolObjBase>();
    //对象池对象
    private Transform poolMgrObj;

    private PoolMgr ()
    {
        InitPoolMgr();
    }
    private void InitPoolMgr()
    {
        if (poolMgrObj == null)
        {
            GameObject MgrObj = new GameObject();
            MgrObj.name = "PoolMgr";
            poolMgrObj = MgrObj.transform;
            GameObject.DontDestroyOnLoad(MgrObj);
        }
    }

    /// <summary>
    /// 根据路径将对象压入对象池
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <param name="obj"></param>
    public void PushObj<T>(string path , T obj) where T : Object
    {

        if (!pools.TryGetValue(path, out PoolObjBase poolObj))
        {
            poolObj = pools[path] = new PoolObj<T>(poolMgrObj, path);
        }

        (poolObj as PoolObj<T>).PushObj(obj);
    }

    /// <summary>
    /// 根据路径弹出对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="path"></param>
    /// <returns></returns>
    public T PopObj<T>(string path) where T : Object
    {
        if (!pools.TryGetValue(path, out PoolObjBase poolObj))
        {
            poolObj = pools[path] = new PoolObj<T>(poolMgrObj, path);
        }

        return (poolObj as PoolObj<T>).PopObj();
    }

    public void PopObjAsync<T>(string path,UnityAction<T> callback = null) where T :Object
    {
        if (!pools.TryGetValue(path, out PoolObjBase poolObj))
        {
            poolObj = pools[path] = new PoolObj<T>(poolMgrObj, path);
        }

        (poolObj as PoolObj<T>).PopObjAsync(callback);
    }

    public void ClearPools()
    {
        foreach (var item in pools)
        {
            item.Value.ClearPool();
        }

        pools.Clear();
    }
}
