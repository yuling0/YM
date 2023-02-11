using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 有限状态机基类（状态控制器）
/// </summary>
public interface IFSM : IState
{
    public void InitFSM();
    public void ChangeState(string stateName);
    public IState GetCurrentState();

}
