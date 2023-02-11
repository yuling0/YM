using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeWidth(500)]
public class SelectionNode : DialogNodeBase
{
    [LabelText("标题")]
    public string title;
    [Output(dynamicPortList = true),OnCollectionChanged("OnDynamicPortListChange")]
    public List<string> options = new List<string>();
}
//[System.Serializable]
//public class OptionInfo
//{
//    [LabelText("选项内容"), LabelWidth(100)]
//    public string optionContent;    //选项文本
//    [LabelText("触发的事件"), LabelWidth(100)]
//    public string eventName;        //选项该触发的事件
//    [LabelText("参数类型"), LabelWidth(100)]
//    public E_ParameterType parameterType;

//    //这里可以只使用字符串类型就可以（之后根据类型强转）这里选择偷懒
//    [ShowIf("@this.parameterType == E_ParameterType.E_Int"),LabelWidth(100)]
//    public int intValue;
//    [ShowIf("@this.parameterType == E_ParameterType.E_String"), LabelWidth(100)]
//    public string strValue;
//    [ShowIf("@this.parameterType == E_ParameterType.E_Float"), LabelWidth(100)]
//    public float floatValue;
//    [ShowIf("@this.parameterType == E_ParameterType.E_Bool"), LabelWidth(100)]
//    public bool boolValue;
//    [ShowIf("@this.parameterType == E_ParameterType.E_Custom"), LabelWidth(100)]

//    //基本用不到
//    [System.NonSerialized, OdinSerialize]
//    public SelectionEventArgs customValue;

//}