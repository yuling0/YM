using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ResourceMgr : SingletonBase<ResourceMgr>
{
    private ResourceMgr()
    {

    }
    public T LoadAsset<T>(string resPath) where T : Object
    {
        T obj = Resources.Load<T>(resPath);

        if(obj is GameObject)
        {
            return GameObject.Instantiate<GameObject>(obj as GameObject) as T;
        }
        if (obj is Component)
        {
            GameObject.Instantiate<GameObject>((obj as Component).gameObject);
        }

        return obj;
    }

    public T[] LoadAllAsset<T>(string resPath) where T : Object 
        => Resources.LoadAll<T>(resPath);

    public void LoadAssetAsync<T>(string resPath , UnityAction<T> callback = null) where T : Object
    {
        MonoMgr.Instance.StartCoroutine(LoadAsync<T>(resPath, callback));
    }

    private IEnumerator LoadAsync<T>(string resPath, UnityAction<T> callback) where T : Object
    {
        ResourceRequest resourceRequest =  Resources.LoadAsync(resPath);

        yield return resourceRequest;

        T obj = resourceRequest.asset as T;

        if (resourceRequest.asset is GameObject)
        {
            GameObject go = GameObject.Instantiate<GameObject>(resourceRequest.asset as GameObject);

            if(typeof(T) == typeof(GameObject))
            {
                obj = go as T;
            }
            else if (typeof(Component).IsAssignableFrom(typeof(T)))
            {
                obj = go.GetComponentInChildren<T>();
                Debug.Log(obj.name);
            }
        }

        callback?.Invoke(obj);
    }


    public void UnLoadAsset(object obj)
    {
        Resources.UnloadAsset(obj as Object);
    }

    public void UnloadUnusedAssets()
    {
        Resources.UnloadUnusedAssets();
    }
}
