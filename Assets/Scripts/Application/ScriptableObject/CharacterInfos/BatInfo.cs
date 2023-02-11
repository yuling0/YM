using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="YM/CharacterInfo/BatInfo",fileName ="BatInfo")]
public class BatInfo : MonsterInfo
{

    [TabGroup("属性", "移动相关属性")]
    [LabelText("冲刺速度")]
    public float sprintSpeed;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("巡逻范围（圆形）")]
    public float patrolRange;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("需要距离玩家的高度")]
    public Vector2 optimalHight;

}
