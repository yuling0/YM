using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "YM/CharacterInfo/WolfInfo",fileName = "WolfInfo")]
public class WolfInfo : MonsterInfo
{
    [TabGroup("����", "ս���������")]
    [LabelText("������λ���ٶ�")]
    public float comboAttackVelocity;

}
