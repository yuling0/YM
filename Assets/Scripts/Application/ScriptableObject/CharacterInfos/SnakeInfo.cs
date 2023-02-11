using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "YM/CharacterInfo/SnakeInfo", fileName = "SnakeInfo")]
public class SnakeInfo : MonsterInfo
{
    [TabGroup("����", "�ƶ��������")]
    [LabelText("����ٶ�����")]
    public Vector2 sprintSpeed;

    [TabGroup("����", "ս���������")]
    [LabelText("��ͨҧ�ľ���")]
    public float biteRange;


    [TabGroup("����", "ս���������")]
    [LabelText("��Χҧ�ľ���")]
    public float wideBiteRange;

    [TabGroup("����", "ս���������")]
    [LabelText("��Χҧ�ļ��")]
    public float wideBiteInterval;
}
