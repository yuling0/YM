using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel : BasePanel
{
    public Image img_MenuTop;
    public Image img_MenuBot;
    public Image img_MenuMid;
    public Text txt_Title;
    public Text txt_Page;
    public Image img_Selector;
    public Text txt_Description;
    public Transform _inventoryList;

    private SelectionWindow selectionWindow;
    private UIItemSlot[] _uiItemSlots;      //背包格子对象
    public int curSelectIndex;             //当前选中的索引
    public int _maxDisplayCount;           //背包一页最多显示数量

    private int _curPage = 0;              //当前显示的背包页数
    private int _maxPage;                  //当前背包的最大页数

    public override void OnInit()
    {
        base.OnInit();
		img_MenuTop = GetControl<Image>("img_MenuTop");
		img_MenuBot = GetControl<Image>("img_MenuBot");
		img_MenuMid = GetControl<Image>("img_MenuMid");
		txt_Title = GetControl<Text>("txt_Title");
		txt_Page = GetControl<Text>("txt_Page");
		img_Selector = GetControl<Image>("img_Selector");
        txt_Description = GetControl<Text>("txt_Description");
        _inventoryList = transform.FindChildTF("InventoryList");

        _uiItemSlots = _inventoryList.GetComponentsInChildren<UIItemSlot>();
        HideInventoryItems();

        _maxDisplayCount = 8;
        curSelectIndex = 0;

        selectionWindow = GetControl<SelectionWindow>("InventoryBG");
        selectionWindow.OnConfirmSelect += ConfirmSelect;
        selectionWindow.OnMoveSelector += MoveSelector;
        selectionWindow.OnPageChanged += UpdateItemSlot;
        _showPanelSequence.OnComplete(InitInventoryList);
    }



    public override void OnOpen()
    {
        base.OnOpen();
        _showPanelSequence.Restart();
        _curPage = 0;
        SoundManager.Instance.PlaySound(E_SoundType.UI, Consts.C_Select);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (InputMgr.Instance.GetKeyDown(Consts.K_Jump))
        {
            UIManager.Instance.Pop();
        }

    }

    public override void OnHide()
    {
        base.OnHide();
        _hidePanelSequence.Restart();
        HideInventoryItems();
        SoundManager.Instance.PlaySound(E_SoundType.UI, Consts.C_Back);
        EventMgr.Instance.OnEventTrigger(Consts.E_DialogContinue);
    }

    private void HideInventoryItems()
    {
        foreach (var item in _uiItemSlots)
        {
            item.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 初始化背包第0页
    /// </summary>
    private void InitInventoryList()
    {
        var itemSlots = InventoryMgr.Instance.GetInventoryItems;

        int len = Mathf.Min(_maxDisplayCount, itemSlots.Count);   //当前页显示物品数量

        selectionWindow.SetCount(InventoryMgr.Instance.GetInventoryItems.Count);

        UpdateItemSlot(0, len);
        UpdateDescription(0);
    }

    /// <summary>
    /// 更新背包
    /// </summary>
    /// <param name="page">页数</param>
    /// <param name="count">需要显示的数量</param>
    private void UpdateItemSlot(int page,int count)
    {
        var itemSlots = InventoryMgr.Instance.GetInventoryItems;

        for (int i = 0; i < count; i++)
        {
            _uiItemSlots[i].Init(itemSlots[i + page * _maxDisplayCount]);
        }

        for (int i = 0; i < _uiItemSlots.Length; i++)
        {
            _uiItemSlots[i].gameObject.SetActive(i < count);
        }
    }

    private void ConfirmSelect(int index)
    {
        Debug.Log(InventoryMgr.Instance.GetInventoryItems[index].GetItem.itemName);
        InventoryMgr.Instance.RemoveItem(index);
        selectionWindow.SetCount(InventoryMgr.Instance.GetInventoryItems.Count);
    }

    private void MoveSelector(int index)
    {
        UpdateDescription(index);
        SoundManager.Instance.PlaySound(E_SoundType.UI, Consts.C_MoveSelector);
    }

    private void UpdateDescription(int index)
    {
        txt_Description.text = InventoryMgr.Instance.GetInventoryItems[index].GetItem.Description;
    }
}