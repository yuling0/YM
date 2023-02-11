using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitInfo : SerializedScriptableObject
{
    [FoldoutGroup("角色属性")]
    [HorizontalGroup("角色属性/HG", LabelWidth = 100, Width = 0.5f)]
    [VerticalGroup("角色属性/HG/Left")]
    [PreviewField(150, ObjectFieldAlignment.Center), HideLabel]
    public Texture2D icon;
    [VerticalGroup("角色属性/HG/Right"), PropertyOrder(0f), LabelText("单位名称")]
    public string unitName;
}
