using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "YM/CharacterInfo/SnakeInfo", fileName = "SnakeInfo")]
public class SnakeInfo : MonsterInfo
{
    [TabGroup("属性", "移动相关属性")]
    [LabelText("冲刺速度向量")]
    public Vector2 sprintSpeed;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("普通咬的距离")]
    public float biteRange;


    [TabGroup("属性", "战斗相关属性")]
    [LabelText("大范围咬的距离")]
    public float wideBiteRange;

    [TabGroup("属性", "战斗相关属性")]
    [LabelText("大范围咬的间隔")]
    public float wideBiteInterval;
}
