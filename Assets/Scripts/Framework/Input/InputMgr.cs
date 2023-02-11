using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMgr : SingletonBase<InputMgr>
{
    private KeyData keyData;
    public InputMgr()
    {
        MonoMgr.Instance.AddUpateAction(OnUpdate);
        //TODO:后续可能添加改键
        keyData = Resources.Load<KeyData>("ScriptableObjects/KeyData"); //加载初始按键
    }
    public bool GetDoubleKey(string keyName)
    {
        return keyData.GetDoubleKey(keyName);
    }

    public bool GetKeyDown(string keyName)
    {
        return keyData.GetKeyDown(keyName);
    }

    public bool GetKeyStay(string keyName)
    {
        return keyData.GetKeyStay(keyName);
    }

    public bool GetKeyUp(string keyName)
    {
        return keyData.GetKeyUp(keyName);
    }

    public bool GetKeyDownExtend(string keyName)
    {
        return keyData.GetKeyDownExtend(keyName);
    }

    public bool GetKeyUpExtend(string keyName)
    {
        return keyData.GetKeyUpExtend(keyName);
    }

    public bool GetKeyDelay(string keyName)
    {
        return keyData.GetKeyDelay(keyName);
    }
    public void OnUpdate()
    {
        keyData?.OnUpdate();
    }
}
