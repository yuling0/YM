using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName ="YM/CharacterInfo/ErQieInfo",fileName ="ErQieInfo", order = 1)]
public class ArcheInfo : CombatInfo
{
    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(2f),LabelText("蓝量")]
    public int mp;

    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(0f),LabelText("等级")]
    public int level;

    [TabGroup("属性", "基本属性")]
    [HorizontalGroup("属性/基本属性/HG2", LabelWidth = 100)]
    [VerticalGroup("属性/基本属性/HG2/Left"),LabelText("精神力")]
    public int spirit;

    [VerticalGroup("属性/基本属性/HG2/Left"), LabelText("抵抗力")]
    public int resistance;

    [VerticalGroup("属性/基本属性/HG2/Left"), LabelText("当前经验值")]
    public int currentExperience;

    [VerticalGroup("属性/基本属性/HG2/Left"), LabelText("所需经验值")]
    public int requireExperience;

    [VerticalGroup("属性/基本属性/HG2/Right"), LabelText("行走速度")]
    public float walkSpeed;

    [VerticalGroup("属性/基本属性/HG2/Right"), LabelText("跑步速度")]
    public float runSpeed;

    [VerticalGroup("属性/基本属性/HG2/Right"), LabelText("空中速度")]
    public float airSpeed;

    [VerticalGroup("属性/基本属性/HG2/Right"), LabelText("跳跃力")]
    public float jumpForce;

    [VerticalGroup("属性/基本属性/HG2/Right"), LabelText("最大缓冲速度")]
    public float maxBufferSpeed;


    [TabGroup("属性", "技能属性"),LabelText("骑士突刺的位移速度")]
    public float KnightThrustVelocity;

    [TabGroup("属性", "技能属性"), LabelText("跳跃回旋斩的速度向量")]
    public Vector3 LeapingSlashVelocity;


    [TabGroup("属性", "技能属性"), LabelText("大范围斩击的位移速度")]
    public float WideSlashVelocity;


    [TabGroup("属性", "技能属性"), LabelText("跳跃踢的速度向量")]
    public Vector3 JumpKickVelocity;



    [TabGroup("属性", "技能属性"), LabelText("跳跃踢后缓冲的速度")]
    public float JumpKickBufferVelocity;

    [TabGroup("属性", "技能属性"), LabelText("跳跃回旋斩后缓冲的速度")]
    public float LeapingSlashBufferVelocity;


    [TabGroup("属性", "技能属性"), LabelText("前翻滚的速度")]
    public float ForwardRollVelocity;

    [TabGroup("属性", "技能属性"), LabelText("后翻滚的速度")]
    public float BackwardRollVelocity;


    [TabGroup("属性", "技能属性"), LabelText("连击前斩的速度")]
    public float ComboAttackForwardVelocity;

    [TabGroup("属性", "技能属性"), LabelText("连击上斩的速度")]
    public float ComboAttackUpVelocity;

    [TabGroup("属性", "技能属性"), LabelText("反向跳跃回旋斩的速度向量")]
    public Vector2 BackwardLeapingSlashVelocity;

    [TabGroup("属性", "技能属性"), LabelText("车轮斩的速度")]
    public float WheelSlashVelocity;

}
