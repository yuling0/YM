using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoMgr : SingletonBase<MonoMgr>
{
    MonoController mono;
    private MonoMgr()
    {
        GameObject MonoController = new GameObject();
        MonoController.name = "MonoController";
        mono = MonoController.AddComponent<MonoController>();
        GameObject.DontDestroyOnLoad(MonoController);
    }

    public void AddUpateAction(UnityAction action)
    {
        mono.AddUpateAction(action);

    }
    public void RemoveUpdateAction(UnityAction action)
    {
        mono.RemoveUpdateAction(action);
    }

    public void AddFixedUpateAction(UnityAction action)
    {
        mono.AddFixedUpateAction(action);
    }
    public void RemoveFixedUpdateAction(UnityAction action)
    {
        mono.RemoveFixedUpdateAction(action);
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return mono.StartCoroutine(routine);
    }

    public void StopCoroutine(IEnumerator routine)
    {
        mono.StopCoroutine(routine);
    }

    public void StopAllCoroutine()
    {
        mono.StopAllCoroutines();
    }

}
