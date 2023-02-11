using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO:只有Idle和Walk状态下才能进行对话
public class DialogPanel : BasePanel
{

    public Image img_Icon;
    public Text txt_RoleName;
    public Text txt_Content;

    public RectTransform DialogBox;
    public RectTransform Pointer;
    public float width;
    public float height;

    private float sentenceIntervalTime = 0.3f;
    public float wordIntervalTime = 0.12f;
    //Test
    DialogNode data;
    int dialogIndex = 0;
    int contentIndex = 0;
    Tween cur;
    private Vector2 screenPos;
    private bool canUpdate;

    private int curSoundId;
    public override void OnInit()
    {
        base.OnInit();
        DialogBox = transform.Find("DialogBox") as RectTransform;
        Pointer = DialogBox.Find("Pointer") as RectTransform;
		img_Icon = GetControl<Image>("img_Icon");
		txt_RoleName = GetControl<Text>("txt_RoleName");
		txt_Content = GetControl<Text>("txt_Content");
        _showPanelSequence.OnComplete(InitDialogWindow);
    }
    public override void OnOpen()
    {
        base.OnOpen();
        img_Icon.SetAlpha(0);
        txt_RoleName.text = "";
        txt_Content.text = "";
        GameManager.Instance.DisablePlayerControl();
        _showPanelSequence.Restart();
    }

    public override void OnHide()
    {
        base.OnHide();
        _hidePanelSequence.Restart();
        Pointer.gameObject.SetActive(false);
        GameManager.Instance.EnablePlayerControl();
        canUpdate = false;
    }

    public void SetData(DialogNode data,Vector2 screenPos)
    {
        this.data = data;
        this.screenPos = screenPos;
        dialogIndex = 0;
        curSoundId = -1;
        width = DialogBox.rect.size.x;
        height = DialogBox.rect.size.y;
    }

    public void InitDialogWindow()
    {
        canUpdate = true;

        //设置对话框指针位置
        Pointer.gameObject.SetActive(true);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(DialogBox, screenPos, null, out var pointerPos);
        pointerPos.y = Pointer.localPosition.y;
        Pointer.localPosition = pointerPos;

        //设置对话信息
        img_Icon.enabled = true;
        img_Icon.DoFade(1, sentenceIntervalTime);
        txt_RoleName.text = data.roleName;
        ProcessingDialogContent();

    }

    private void ProcessingDialogContent()
    {
        img_Icon.sprite = data.content[dialogIndex].icon;
        TweenerSequence sequence = TweenManager.Sequence();
        if (data.content[dialogIndex].isNeedPlaySound)
        {
            if (curSoundId >= 0)
            {
                SoundManager.Instance.StopSound(curSoundId);
                curSoundId = -1;
            }
            curSoundId = SoundManager.Instance.PlaySound(E_SoundType.Story, data.content[dialogIndex].soundName);
            sequence.Append(txt_Content.AddText("", data.content[dialogIndex].realContent[0], data.content[dialogIndex].realContent[0].Length * wordIntervalTime));

            for (int i = 1; i < data.content[dialogIndex].realContent.Count; i++)
            {
                sequence.AppendInterval(sentenceIntervalTime);
                sequence.Append(txt_Content.AddText(data.content[dialogIndex].realContent[i], data.content[dialogIndex].realContent[i].Length * wordIntervalTime));
            }
        }
        else
        {
            if (curSoundId >= 0)
            {
                SoundManager.Instance.StopSound(curSoundId);
                curSoundId = -1;
            }
            sequence.Append(txt_Content.AddText("", data.content[dialogIndex].realContent[0], data.content[dialogIndex].realContent[0].Length * wordIntervalTime).OnAddText(() =>
            {
                SoundManager.Instance.PlaySound(E_SoundType.Other, "DialogSound");
            }));

            for (int i = 1; i < data.content[dialogIndex].realContent.Count; i++)
            {
                sequence.AppendInterval(sentenceIntervalTime);
                sequence.Append(txt_Content.AddText(data.content[dialogIndex].realContent[i], data.content[dialogIndex].realContent[i].Length * wordIntervalTime).OnAddText(() =>
                {
                    SoundManager.Instance.PlaySound(E_SoundType.Other, "DialogSound");
                }));
            }
        }
        sequence.OnComplete(() => { dialogIndex++; });
        cur = sequence;
    }

    public override void OnUpdate()
    {
        if (!canUpdate) return;
        if (dialogIndex < data.content.Count && Input.GetKeyDown(KeyCode.V))
        {
            if (cur.IsPlaying)
            {
                txt_Content.text = data.content[dialogIndex].content;
                cur.Kill();
                dialogIndex++;
            }
            else
            {
                ProcessingDialogContent();
            }
            
        }
        else if (dialogIndex >= data.content.Count && Input.GetKeyDown(KeyCode.V))
        {
            if (curSoundId >= 0)
            {
                SoundManager.Instance.StopSound(curSoundId);
                curSoundId = -1;
            }
            if (cur.IsPlaying)
            {
                txt_Content.text = data.content[dialogIndex].content;
                cur.Kill();
            }
            else
            {
                UIManager.Instance.Pop();
                EventMgr.Instance.OnEventTrigger(Consts.E_DialogContinue);
            }

        }
    }
}