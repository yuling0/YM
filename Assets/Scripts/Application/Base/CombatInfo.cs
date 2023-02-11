using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CombatInfo : UnitInfo
{
    //[FoldoutGroup("��ɫ����")]
    //[HorizontalGroup("��ɫ����/HG",LabelWidth =100,Width = 0.5f)]
    //[VerticalGroup("��ɫ����/HG/Left")]
    //[PreviewField(150,ObjectFieldAlignment.Center),HideLabel]
    //public Texture2D icon;

    [VerticalGroup("��ɫ����/HG/Right"),PropertyOrder(1f),LabelText("Ѫ��")]
    public int hp;

    [VerticalGroup("��ɫ����/HG/Right"), PropertyOrder(3f), LabelText("������")]
    public int atk;

    [VerticalGroup("��ɫ����/HG/Right"), PropertyOrder(4f), LabelText("������")]
    public int def;

    [VerticalGroup("��ɫ����/HG/Right"), PropertyOrder(4f), LabelText("���м��Ĳ㼶")]
    public LayerMask targetMask;
}
