using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ʼ���ӿ�
/// </summary>
public interface IInit<T>
{
    void Init(T obj);
}

public interface IInit
{
    void Init();
}
