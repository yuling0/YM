using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogManager : SingletonBase<DialogManager>
{
    //TODO:这里可能要用空间换时间：方案1：将所有对话数据一开始加载到内存。 方案2：根据章节加载对话数据，章节切换时重新加载。
    private Dictionary<string, DialogDataContainer> dic;
    private DialogDataContainer dialogData;
    private DialogNodeBase curNode;
    private UnitManager unitManager;
    private bool isInDialog;
    private bool isEventTrigger;    //是否是事件触发的对话
    public bool IsInDialog => isInDialog;
    private DialogManager()
    {
        unitManager = UnitManager.Instance;
        dic = new Dictionary<string, DialogDataContainer>();
        InitDialogData();
        EventMgr.Instance.AddEventListener(Consts.E_DialogContinue, Continue);
        EventMgr.Instance.AddEventListener<int>(Consts.E_OnSelectionPanelConfirm, OnSelectionPanelConfirm);
        EventMgr.Instance.AddEventListener<StoryEventArgs>(Consts.E_StartDialog, StartDialog);
    }
    private void InitDialogData()
    {
       var dataArray = ResourceMgr.Instance.LoadAllAsset<DialogDataContainer>(Consts.P_DialogDataPath);
        foreach (var dialogData in dataArray)
        {
            dic.Add(dialogData.name, dialogData);
        }
    }
    public void StartDialog(DialogDataContainer dialogData)
    {
        this.dialogData = dialogData;
        isInDialog = true;
        curNode = dialogData.root.GetOutputPort("OutputPort").GetConnection(0).node as DialogNodeBase;
        //根据节点类型写逻辑
        if (curNode is DialogNode)
        {
            ProcessDialogNode();
        }
        else if (curNode is SelectionNode)
        {
            ProcessSelectionNode();
        }
        else if(curNode is EventNode)
        {
            ProcessEventNode();
        }
        else if (curNode is EndNode)
        {
            isInDialog = false;
            EventMgr.Instance.OnEventTrigger("OnDialogEnd", dialogData.name);
        }
    }
    //TODO:这里要处理对话结束的回调
    public void StartDialog(StoryEventArgs args)
    {
        if (dic.TryGetValue(args.stringArgs, out DialogDataContainer dialogData))
        {
            StartDialog(dialogData);
        }
        else
        {
            throw new System.Exception($"{args.stringArgs} is involid");
        }
    }

    private void ProcessEventNode()
    {
        EventNode eventNode = curNode as EventNode;
        curNode = curNode.GetOutputPort("OutputPort").GetConnection(0).node as DialogNodeBase;
        switch (eventNode.parameterType)
        {
            case E_EventArgsType.E_Null:
                EventMgr.Instance.OnEventTrigger(eventNode.eventName);
                break;
            case E_EventArgsType.E_String:
                EventMgr.Instance.OnEventTrigger(eventNode.eventName, eventNode.strValue);
                break;
            case E_EventArgsType.E_Int:
                EventMgr.Instance.OnEventTrigger(eventNode.eventName, eventNode.intValue);
                break;
            case E_EventArgsType.E_Float:
                EventMgr.Instance.OnEventTrigger(eventNode.eventName, eventNode.floatValue);
                break;
            case E_EventArgsType.E_Bool:
                EventMgr.Instance.OnEventTrigger(eventNode.eventName, eventNode.boolValue);
                break;
            case E_EventArgsType.E_Custom:
                EventMgr.Instance.OnMultiParameterEventTrigger(eventNode.customValue);
                break;
        }
        
    }

    private void ProcessSelectionNode()
    {
        UIManager.Instance.Push<SelectionPanel>((panel) =>
        {
            panel.SetOptions(curNode as SelectionNode);
        });
    }

    /// <summary>
    /// 处理正常的对话节点
    /// </summary>
    private void ProcessDialogNode()
    {
        //TODO:根据说话人来确定对话框位置（UnitManager）
        Vector2 targetPos = unitManager.GetUnit((curNode as DialogNode).unitId).transform.position + Vector3.up * 1.5f;
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(targetPos);
        float canvasWidth = UIManager.Instance.CanvasWidth;
        UIManager.Instance.Push<DialogPanel>((panel) =>
        {
            panel.SetData(curNode as DialogNode, screenPoint);
            //TODO: 这里改变分辨率，会改变画布的宽高，需要重新计算（后续可能会写一个修改分辨率的事件）
            RectTransformUtility.ScreenPointToLocalPointInRectangle(panel.transform as RectTransform, screenPoint, null, out var dialogPos);
            //Debug.Log(_canvas.sizeDelta);

            //限制对话框的x轴范围，防止对话框超出屏幕外
            dialogPos.x = Mathf.Clamp(dialogPos.x,
                -canvasWidth / 2 + panel.width / 2,
                canvasWidth / 2 - panel.width / 2);

            panel.DialogBox.localPosition = dialogPos;

            curNode = curNode.GetOutputPort("OutputPort").GetConnection(0).node as DialogNodeBase;

        });
        
    }

    public void Continue()
    {
        if (!isInDialog)
        {
            Debug.LogError("当前没有正在进行的对话");
            return;
        }
        //根据节点类型写逻辑
        if (curNode is DialogNode)
        {
            ProcessDialogNode();
        }
        else if (curNode is SelectionNode)
        {
            ProcessSelectionNode();
        }
        else if (curNode is EventNode)
        {
            
        }
        else if (curNode is EndNode)
        {
            isInDialog = false;
            EventMgr.Instance.OnEventTrigger("OnDialogEnd", dialogData.name);
        }
    }

    private void OnSelectionPanelConfirm(int index)
    {
        SelectionNode selectionNode = curNode as SelectionNode;
        curNode = selectionNode.GetOutputPort($"options {index}").GetConnection(0).node as DialogNodeBase;
        ProcessEventNode();
    }

}
