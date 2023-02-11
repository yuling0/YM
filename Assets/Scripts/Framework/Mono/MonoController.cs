using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonoController : MonoBehaviour
{
    private UnityAction updateAction;
    private UnityAction fixedupdateAction;

    public void AddUpateAction(UnityAction action)
    {
        updateAction += action;
    }
    public void RemoveUpdateAction(UnityAction action)
    {
        updateAction -= action;
    }

    public void AddFixedUpateAction(UnityAction action)
    {
        fixedupdateAction += action;
    }
    public void RemoveFixedUpdateAction(UnityAction action)
    {
        fixedupdateAction -= action;
    }

    private void Update()
    {
        updateAction?.Invoke();
    }

    private void FixedUpdate()
    {
        fixedupdateAction?.Invoke();
    }

}
