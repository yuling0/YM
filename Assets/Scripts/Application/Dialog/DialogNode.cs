using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using XNode;

[NodeWidth(600)]
public class DialogNode : DialogNodeBase
{
    //[HorizontalGroup("FirstRow",100),HideLabel, PreviewField(100, ObjectFieldAlignment.Left)]
    //public Sprite icon;             //头像
    public int unitId;          //加载资源的id（Unit的id）

    public string roleName;         //文本中显示的角色名

    //[LabelWidth(120)]
    //public bool isNeedPlaySound;    //是否需要播放音效

    //[LabelWidth(120), ShowIf("@isNeedPlaySound")]
    //public string soundName;        //需要播放的音效名

    //[LabelWidth(120)]
    //public bool isNeedSwitchBGM;    //是否切换bgm

    //[LabelWidth(120), ShowIf("@isNeedSwitchBGM")]
    //public string bgmName;        //需要播放的bgm名

    [Output]
    public bool OutputPort;

    [ListDrawerSettings(Expanded = true,NumberOfItemsPerPage = 4,ShowPaging = true)]

    public List<DialogInfo> content = new List<DialogInfo>();

}
[System.Serializable]
public class DialogInfo
{   
    [HorizontalGroup(90),HideLabel,PreviewField(90, ObjectFieldAlignment.Left)]
    public Sprite icon;

    [HorizontalGroup,HideLabel,Multiline(5),OnValueChanged("SplitContent")]
    public string content;

    [LabelWidth(120)]
    public bool isNeedPlaySound;    //是否需要播放音效

    [LabelWidth(120), ShowIf("@isNeedPlaySound")]
    public string soundName;        //需要播放的音效名
    [ReadOnly]
    public List<string> realContent;

    public void SplitContent()
    {
        List<string> strList = new List<string>();
        string[] strs = content.Split('\n');
        StringBuilder sb = new StringBuilder();
        foreach (var s in strs)
        {
            sb.Clear();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '.' || s[i] == '。' || s[i] == '？' || s[i] == '！')
                {
                    while (i < s.Length && (s[i] == '.' || s[i] == '。' || s[i] == '？' || s[i] == '！'))
                    {
                        sb.Append(s[i]);
                        i++;
                    }
                    if (i != s.Length)
                    {
                        strList.Add(sb.ToString());
                        sb.Clear();
                    }
                    i -= 1;
                }
                else
                {
                    sb.Append(s[i]);
                }

                if (i == s.Length - 1)
                {
                    sb.Append('\n');
                    strList.Add(sb.ToString());
                    sb.Clear();
                }
            }
        }
        realContent = strList;
    }

    //DOTO:这里考虑要不要做对话之后还有事件触发，事件触发之后接着对话的内容（即：不切换下一个对话面板，在同一个对话面板）

    //[LabelText("是否有事件"), LabelWidth(80)]
    //public bool hasEvent;

    //[ShowIf("@hasEvent")]
    //[BoxGroup("Event")]
    //[LabelText("事件名称"), LabelWidth(100)]
    //public string eventName;


    //[ShowIf("@hasEvent")]
    //[BoxGroup("Event")]
    //[LabelText("参数类型"), LabelWidth(100)]
    //public E_ParameterType parameterType;

    //[BoxGroup("Event")]
    //[ShowIf("@hasEvent && this.parameterType == E_ParameterType.E_Int"), LabelWidth(100)]
    //public int intValue;

    //[BoxGroup("Event")]
    //[ShowIf("@hasEvent && this.parameterType == E_ParameterType.E_String"), LabelWidth(100)]
    //public string strValue;

    //[BoxGroup("Event")]
    //[ShowIf("@hasEvent && this.parameterType == E_ParameterType.E_Float"), LabelWidth(100)]
    //public float floatValue;

    //[BoxGroup("Event")]
    //[ShowIf("@hasEvent && this.parameterType == E_ParameterType.E_Bool"), LabelWidth(100)]
    //public bool boolValue;

}
