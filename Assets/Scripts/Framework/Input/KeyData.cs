using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="YM/KeyData",fileName ="KeyData")]
public class KeyData : SerializedScriptableObject
{
    //可以改字典
    public List<NormalKeyInfo> normalKeyInfos = new List<NormalKeyInfo>();
    public List<KeyValueInfo> keyValueInfos = new List<KeyValueInfo>();
    public bool GetKeyDown(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyDown;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public bool GetKeyUp(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyUp;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public bool GetKeyStay(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyStay;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public bool GetDoubleKey(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.DoubleKey;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public bool GetKeyDownExtend(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyDownExtend;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }
    public bool GetKeyUpExtend(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyUpExtend;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public bool GetKeyDelay(string keyName)
    {
        NormalKeyInfo keyInfo = GetNormalKeyInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyDelay;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return false;
    }

    public float GetKeyValue(string keyName)
    {
        KeyValueInfo keyInfo = GetKeyValueInfo(keyName);
        if (keyInfo != null)
        {
            return keyInfo.KeyValue;
        }
        Debug.Log($"未找到名称为:{keyName}的键");
        return 0f;
    }

    private NormalKeyInfo GetNormalKeyInfo(string keyName)
    {
        for (int i = 0; i < normalKeyInfos.Count; i++)
        {
            if(normalKeyInfos[i]._keyName.Equals(keyName))
            {
                return normalKeyInfos[i];
            }
        }

        return null;
    }

    private KeyValueInfo GetKeyValueInfo(string keyName)
    {
        for (int i = 0; i < keyValueInfos.Count; i++)
        {
            if (keyValueInfos[i]._keyName.Equals(keyName))
            {
                return keyValueInfos[i];
            }
        }

        return null;
    }

    public void OnUpdate()
    {
        for (int i = 0; i < normalKeyInfos.Count; i++)
        {
            normalKeyInfos[i].OnUpdate();
        }
    }

}
