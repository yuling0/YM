using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "YM/CharacterInfo/PterosaurInfo", fileName = "PterosaurInfo")]
public class PterosaurInfo : CombatInfo
{
    [TabGroup("属性", "移动属性相关")]
    [LabelText("移动速度")]
    public Vector2 moveSpeed;

    [TabGroup("属性", "移动属性相关")]
    [LabelText("后撤速度")]
    public Vector2 retreatSpeed;

    [TabGroup("属性", "移动属性相关")]
    [LabelText("俯冲前摇速度")]
    public Vector2 diveAnitcipationSpeed;

    [TabGroup("属性", "移动属性相关")]
    [LabelText("俯冲速度")]
    public Vector2 diveSpeed;

    [TabGroup("属性", "移动属性相关")]
    [LabelText("旋转下压速度")]
    public Vector2 smashSpeed;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("近距离AI战斗最大x距离")]
    public float CloseCombatMaxDistanceX;           //在 0 ~ x 距离使用近距离技能 超过x距离尝试使用远距离技能
    [TabGroup("属性", "战斗属性相关")]
    [LabelText("喷火需要与目标x轴的距离范围")]
    public Vector2 jetFlameXAxisDistanceRange;       //x 为能释放该技能的最小距离 ， y为能释放该技能的最大距离

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("喷火需要与目标y轴的距离范围")]
    public Vector2 jetFlameYAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("喷火状态持续时间")]
    public float jetFlameDurationTime;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("生成火焰粒子的间隔时间")]
    public float generateFlameParticleIntervalTime;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("每次发射粒子的数量范围")]
    public Vector2Int emitCountRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("俯冲需要与目标x轴的距离范围")]
    public Vector2 diveXAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("俯冲需要与目标y轴的距离范围")]
    public Vector2 diveYAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("旋转下压需要与目标x轴的距离范围")]
    public Vector2 smashXAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("旋转下压需要与目标Y轴的距离范围")]
    public Vector2 smashYAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("头撞需要与目标x轴的距离范围")]
    public Vector2 headButtXAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("头撞需要与目标y轴的距离范围")]
    public Vector2 headButtYAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("甩尾需要与目标x轴的距离范围")]
    public Vector2 tailFlickXAxisDistanceRange;

    [TabGroup("属性", "战斗属性相关")]
    [LabelText("甩尾需要与目标y轴的距离范围")]
    public Vector2 tailFlickYAxisDistanceRange;
}
