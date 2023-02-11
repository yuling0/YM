using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "YM/CharacterInfo/PterosaurInfo", fileName = "PterosaurInfo")]
public class PterosaurInfo : CombatInfo
{
    [TabGroup("����", "�ƶ��������")]
    [LabelText("�ƶ��ٶ�")]
    public Vector2 moveSpeed;

    [TabGroup("����", "�ƶ��������")]
    [LabelText("���ٶ�")]
    public Vector2 retreatSpeed;

    [TabGroup("����", "�ƶ��������")]
    [LabelText("����ǰҡ�ٶ�")]
    public Vector2 diveAnitcipationSpeed;

    [TabGroup("����", "�ƶ��������")]
    [LabelText("�����ٶ�")]
    public Vector2 diveSpeed;

    [TabGroup("����", "�ƶ��������")]
    [LabelText("��ת��ѹ�ٶ�")]
    public Vector2 smashSpeed;

    [TabGroup("����", "ս���������")]
    [LabelText("������AIս�����x����")]
    public float CloseCombatMaxDistanceX;           //�� 0 ~ x ����ʹ�ý����뼼�� ����x���볢��ʹ��Զ���뼼��
    [TabGroup("����", "ս���������")]
    [LabelText("�����Ҫ��Ŀ��x��ľ��뷶Χ")]
    public Vector2 jetFlameXAxisDistanceRange;       //x Ϊ���ͷŸü��ܵ���С���� �� yΪ���ͷŸü��ܵ�������

    [TabGroup("����", "ս���������")]
    [LabelText("�����Ҫ��Ŀ��y��ľ��뷶Χ")]
    public Vector2 jetFlameYAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("���״̬����ʱ��")]
    public float jetFlameDurationTime;

    [TabGroup("����", "ս���������")]
    [LabelText("���ɻ������ӵļ��ʱ��")]
    public float generateFlameParticleIntervalTime;

    [TabGroup("����", "ս���������")]
    [LabelText("ÿ�η������ӵ�������Χ")]
    public Vector2Int emitCountRange;

    [TabGroup("����", "ս���������")]
    [LabelText("������Ҫ��Ŀ��x��ľ��뷶Χ")]
    public Vector2 diveXAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("������Ҫ��Ŀ��y��ľ��뷶Χ")]
    public Vector2 diveYAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("��ת��ѹ��Ҫ��Ŀ��x��ľ��뷶Χ")]
    public Vector2 smashXAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("��ת��ѹ��Ҫ��Ŀ��Y��ľ��뷶Χ")]
    public Vector2 smashYAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("ͷײ��Ҫ��Ŀ��x��ľ��뷶Χ")]
    public Vector2 headButtXAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("ͷײ��Ҫ��Ŀ��y��ľ��뷶Χ")]
    public Vector2 headButtYAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("˦β��Ҫ��Ŀ��x��ľ��뷶Χ")]
    public Vector2 tailFlickXAxisDistanceRange;

    [TabGroup("����", "ս���������")]
    [LabelText("˦β��Ҫ��Ŀ��y��ľ��뷶Χ")]
    public Vector2 tailFlickYAxisDistanceRange;
}
