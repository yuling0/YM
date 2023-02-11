using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.SceneManager;
public class SceneLoadManager : SingletonBase<SceneLoadManager>
{
    public string curSceneName => GetActiveScene().name;
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneAsync(string sceneName , UnityAction callback = null)
    {
        MonoMgr.Instance.StartCoroutine(LoadSceneAsyncInternal(sceneName, callback));
    }

    private IEnumerator LoadSceneAsyncInternal(string sceneName, UnityAction callback)
    {
        var ac = SceneManager.LoadSceneAsync(sceneName);
        yield return ac;
        callback?.Invoke();
    }
}
