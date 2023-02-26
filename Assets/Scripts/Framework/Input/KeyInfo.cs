using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public abstract class KeyInfo
{
    [LabelText("����"),ValueDropdown("keyNameList")]
    public string _keyName;
    public KeyCode _keyCode;

    private static IEnumerable keyNameList = new ValueDropdownList<string>()
    {
        { "���",Consts.K_Left },
        { "�Ҽ�",Consts.K_Right },
        { "�ϼ�",Consts.K_Up },
        { "�¼�",Consts.K_Down },
        { "������",Consts.K_Attack },
        { "��Ծ��",Consts.K_Jump },
        { "�ν���",Consts.K_DrawSword },
        { "�˵�",Consts.K_Menu }
    };
    public KeyInfo(KeyCode kc)
    {
        _keyCode = kc;
    }
    public abstract void OnUpdate();

    public abstract void Disable();

}
