using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ����״̬�����ࣨ״̬��������
/// </summary>
public interface IFSM : IState
{
    public void InitFSM();
    public void ChangeState(string stateName);
    public IState GetCurrentState();

}
