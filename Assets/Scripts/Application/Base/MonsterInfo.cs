using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterInfo : CombatInfo
{
    [TabGroup("属性", "移动相关属性")]
    [LabelText("移动速度")]
    public float moveSpeed;

    [TabGroup("属性", "移动相关属性")]
    [LabelText("跳跃力")]
    public float jumpForce;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("检测玩家的范围")]
    public float detectionRange;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("追击范围")]
    public float ChaseRange;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("攻击玩家的范围")]
    public float attackRange;


    [TabGroup("属性", "战斗相关属性")]
    [LabelText("攻击间隔")]
    public float attackInterval;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("与玩家保持距离的范围(水平距离)")]
    public Vector2 optimalRange;

}
