using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : BasePanel
{
    public Image img_MenuTop;
    public Image img_MenuMid;
    public Image img_MenuBot;
    public Image img_Selector;
    public Text txt_Title;

    public SelectionWindow _mainMenu;
    public override void OnInit()
    {
        base.OnInit();
		img_MenuTop = GetControl<Image>("img_MenuTop");
		img_MenuMid = GetControl<Image>("img_MenuMid");
		img_MenuBot = GetControl<Image>("img_MenuBot");
		img_Selector = GetControl<Image>("img_Selector");


        _mainMenu = GetControl<SelectionWindow>("MainMenu");
        _mainMenu.OnConfirmSelect += ConfirmSelector;
        _mainMenu.SetCount(7);
    }


    public override void OnOpen()
    {
        base.OnOpen();
        _showPanelSequence.Restart();                               //显示面板序列动画播放
        SoundManager.Instance.PlaySound(E_SoundType.UI, Consts.C_Select);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (InputMgr.Instance.GetKeyDown(Consts.K_Menu))
        {
            UIManager.Instance.Pop();
        }
    }
    public override void OnHide()
    {
        base.OnHide();
        _hidePanelSequence.Restart();
        SoundManager.Instance.PlaySound(E_SoundType.UI, Consts.C_Back);
    }
    private void ConfirmSelector(int index)
    {

        if(index == 0)
        {
            UIManager.Instance.Push<InventoryPanel>();
        }
    }
}