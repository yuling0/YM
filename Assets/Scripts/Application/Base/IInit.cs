using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 初始化接口
/// </summary>
public interface IInit<T>
{
    void Init(T obj);
}

public interface IInit
{
    void Init();
}
