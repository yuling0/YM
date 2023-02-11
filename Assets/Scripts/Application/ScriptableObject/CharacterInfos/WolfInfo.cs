using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "YM/CharacterInfo/WolfInfo",fileName = "WolfInfo")]
public class WolfInfo : MonsterInfo
{
    [TabGroup("属性", "战斗相关属性")]
    [LabelText("连击的位移速度")]
    public float comboAttackVelocity;

}
