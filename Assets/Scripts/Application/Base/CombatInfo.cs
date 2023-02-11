using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatInfo : UnitInfo
{
    //[FoldoutGroup("角色属性")]
    //[HorizontalGroup("角色属性/HG",LabelWidth =100,Width = 0.5f)]
    //[VerticalGroup("角色属性/HG/Left")]
    //[PreviewField(150,ObjectFieldAlignment.Center),HideLabel]
    //public Texture2D icon;

    [VerticalGroup("角色属性/HG/Right"),PropertyOrder(1f),LabelText("血量")]
    public int hp;

    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(3f), LabelText("攻击力")]
    public int atk;

    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(4f), LabelText("防御力")]
    public int def;

    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(4f), LabelText("击中检测的层级")]
    public LayerMask targetMask;
}
