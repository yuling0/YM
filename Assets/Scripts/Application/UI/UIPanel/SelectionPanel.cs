using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionPanel : BasePanel
{
    public Text txt_Title;
    public RectTransform content;
    public RectTransform dialogBox;
    private SelectionWindow selectionWindow;
    private SelectionNode curNode;
    private Text[] optionsText;
    private float height;
    private int optionCount;
    public override void OnInit()
    {
        base.OnInit();
        txt_Title = GetControl<Text>("txt_Title");
        content = transform.FindChildTF("Content") as RectTransform;
        dialogBox = transform.FindChildTF("DialogBox") as RectTransform;
        optionsText = content.GetComponentsInChildren<Text>();
        height = dialogBox.rect.height;
        selectionWindow = GetControl<SelectionWindow>("DialogBox");
        selectionWindow.OnConfirmSelect += OnConfirmSelect;
        _showPanelSequence.OnComplete(InitOptions);
    }
    public void SetOptions(SelectionNode selectionNode)
    {
        curNode = selectionNode;
        optionCount = curNode.options.Count;
        if (optionCount > 2)
        {
            optionCount = Mathf.Clamp(optionCount, 2, 4);
        }
        dialogBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + (optionCount - 2) * 65f);
    }
    private void InitOptions()
    {

        for (int i = 0; i < 4; i++)
        {
            if (i < optionCount)
            {
                optionsText[i].gameObject.SetActive(true);
                optionsText[i].text = curNode.options[i];
            }
            else
            {
                optionsText[i].gameObject.SetActive(false);
            }
        }
        selectionWindow.SetCount(optionCount);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        _showPanelSequence.Restart();
    }

    public override void OnHide()
    {
        base.OnHide();
        _hidePanelSequence.Restart();
    }

    private void OnConfirmSelect(int index)
    {
        Debug.Log($"选项{index}");
        UIManager.Instance.Pop();
        EventMgr.Instance.OnEventTrigger(Consts.E_OnSelectionPanelConfirm, index);
        
    }
}