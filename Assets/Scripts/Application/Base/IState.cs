using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 状态接口
/// </summary>
public interface IState
{
    void OnEnter();         //状态进入

    void OnUpdate();        //状态逻辑更新

    void OnFixedUpdate();   //状态物理更新

    void OnConditionUpdate();//状态切换条件更新

    void AnimationEventTrigger();//动画事件触发函数

    void OnExit();          //状态退出
}
