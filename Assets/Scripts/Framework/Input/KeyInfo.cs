using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class KeyInfo
{
    [LabelText("¼üÃû"),ValueDropdown("keyNameList")]
    public string _keyName;
    public KeyCode _keyCode;

    private static IEnumerable keyNameList = new ValueDropdownList<string>()
    {
        { "×ó¼ü",Consts.K_Left },
        { "ÓÒ¼ü",Consts.K_Right },
        { "ÉÏ¼ü",Consts.K_Up },
        { "ÏÂ¼ü",Consts.K_Down },
        { "¹¥»÷¼ü",Consts.K_Attack },
        { "ÌøÔ¾¼ü",Consts.K_Jump },
        { "°Î½£¼ü",Consts.K_DrawSword },
        { "²Ëµ¥",Consts.K_Menu }
    };
    public KeyInfo(KeyCode kc)
    {
        _keyCode = kc;
    }
    public abstract void OnUpdate();
}
