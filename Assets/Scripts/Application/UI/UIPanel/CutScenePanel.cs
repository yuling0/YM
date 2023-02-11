using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutScenePanel : BasePanel
{
    public Image img_CutScene1_BackgroundImageTop;
    public Image img_CutScene1_BackgroundImageDown;
    private Vector3 hideScaleVector = new Vector3(1,0,1);
    public float playTime = 0.5f;

    private TweenerSequence hideSceneSequence1;
    private TweenerSequence hideSceneSequence2;
    private TweenerSequence hideSceneSequence3;

    private TweenerSequence showSceneSequence1;
    private TweenerSequence showSceneSequence2;
    private TweenerSequence showSceneSequence3;
    public override void OnInit()
    {
        //base.OnInit();
        img_CutScene1_BackgroundImageTop = GetControl<Image>("img_CutScene1_BackgroundImageTop");
        img_CutScene1_BackgroundImageDown = GetControl<Image>("img_CutScene1_BackgroundImageDown");
        EventMgr.Instance.AddEventListener(Consts.E_OnSceneLoaded, () => { UIManager.Instance.Pop(); });
        InitCutSceneAnimation();
    }

    private void InitCutSceneAnimation()
    {
        hideSceneSequence1 = TweenManager.Sequence();
        hideSceneSequence1.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(2, playTime));
        hideSceneSequence1.OnComplete(OnHideSceneComplete);

        hideSceneSequence2 = TweenManager.Sequence();
        hideSceneSequence2.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(1, playTime));
        hideSceneSequence2.Join(img_CutScene1_BackgroundImageDown.transform.DoScaleY(1, playTime));
        hideSceneSequence2.OnComplete(OnHideSceneComplete);


        hideSceneSequence3 = TweenManager.Sequence();
        hideSceneSequence3.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(1, playTime));
        hideSceneSequence3.Join(img_CutScene1_BackgroundImageDown.transform.DoScaleY(1, playTime));
        hideSceneSequence3.OnComplete(OnHideSceneComplete);


        showSceneSequence1 = TweenManager.Sequence();
        showSceneSequence1.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime));
        showSceneSequence1.OnComplete(() => { OnShowSceneComplete(); PoolMgr.Instance.PushObj(Consts.P_UI + this.GetType().Name, this.gameObject); });


        showSceneSequence2 = TweenManager.Sequence();
        showSceneSequence2.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime));
        showSceneSequence2.Join(img_CutScene1_BackgroundImageDown.transform.DoScaleY(0, playTime));
        showSceneSequence2.OnComplete(() => { OnShowSceneComplete(); PoolMgr.Instance.PushObj(Consts.P_UI + this.GetType().Name, this.gameObject); });

        showSceneSequence3 = TweenManager.Sequence();
        showSceneSequence3.Append(img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime));
        showSceneSequence3.Join(img_CutScene1_BackgroundImageDown.transform.DoScaleY(0, playTime));
        showSceneSequence3.OnComplete(() => { OnShowSceneComplete(); PoolMgr.Instance.PushObj(Consts.P_UI + this.GetType().Name, this.gameObject); });

        hideSceneSequence1.SetAutoKill(false);
        hideSceneSequence2.SetAutoKill(false);
        hideSceneSequence3.SetAutoKill(false);
        showSceneSequence1.SetAutoKill(false);
        showSceneSequence2.SetAutoKill(false);
        showSceneSequence3.SetAutoKill(false);
        hideSceneSequence1.Pause();
        hideSceneSequence2.Pause();
        hideSceneSequence3.Pause();
        showSceneSequence1.Pause();
        showSceneSequence2.Pause();
        showSceneSequence3.Pause();
        hideSceneSequence1.SetUpdate(true);
        hideSceneSequence2.SetUpdate(true);
        hideSceneSequence3.SetUpdate(true);
        showSceneSequence1.SetUpdate(true);
        showSceneSequence2.SetUpdate(true);
        showSceneSequence3.SetUpdate(true);
    }

    public override void OnOpen()
    {
        base.OnOpen();
        int next = Random.Range(1, 4);
        switch (next)
        {
            case 1:
                Hide1();
                break;
            case 2:
                Hide2();
                break;
            case 3:
                Hide3();
                break;
            default:
                break;
        }
    }
    public override void OnHide()
    {
        base.OnHide();
        ShowScene();
    }
    public void ShowScene()
    {
        int next = Random.Range(1, 4);
        switch (next)
        {
            case 1:
                Show1();
                break;
            case 2:
                Show2();
                break;
            case 3:
                Show3();
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 隐藏场景动画1：从上至下隐藏
    /// </summary>
    public void Hide1()
    {
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = Vector2.zero;
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = hideScaleVector;
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = hideScaleVector;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(2, playTime);
        hideSceneSequence1.Restart();
    }
    /// <summary>
    /// 隐藏场景动画2：上下同时向中间合并
    /// </summary>
    public void Hide2()
    {
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = Vector2.zero;
        //这里写死y = -1440 是因为Canvas设置了以高来匹配屏幕大小，Canvas的height始终为1440（参考分辨率），其他模式记得获取Canvas的高度来设置位置
        img_CutScene1_BackgroundImageDown.rectTransform.anchoredPosition = new Vector2 { x = 0, y = -1440 };
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = hideScaleVector;
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = hideScaleVector;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(1, playTime);
        //img_CutScene1_BackgroundImageDown.transform.DoScaleY(1, playTime);
        hideSceneSequence2.Restart();

    }
    /// <summary>
    /// 隐藏场景动画3：中间往两边扩散
    /// </summary>
    public void Hide3()
    {
        //这里写死y = -720 是因为Canvas设置了以高来匹配屏幕大小，Canvas的height始终为1440（参考分辨率），其他模式记得获取Canvas的高度来设置位置
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = new Vector2 { x = 0 , y = -720 };
        img_CutScene1_BackgroundImageDown.rectTransform.anchoredPosition = new Vector2 { x = 0, y = -720 };
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = hideScaleVector;
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = hideScaleVector;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(1, playTime);
        //img_CutScene1_BackgroundImageDown.transform.DoScaleY(1, playTime);
        hideSceneSequence3.Restart();
    }
    public void Show1() 
    {
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = Vector2.zero;
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = new Vector3(1,2,1);
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = Vector3.zero;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime);
        showSceneSequence1.Restart();
    }

    public void Show2()
    {
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = Vector2.zero;
        //这里写死y = -1440 是因为Canvas设置了以高来匹配屏幕大小，Canvas的height始终为1440（参考分辨率），其他模式记得获取Canvas的高度来设置位置
        img_CutScene1_BackgroundImageDown.rectTransform.anchoredPosition = new Vector2 { x = 0, y = -1440 };
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = Vector3.one;
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = Vector3.one;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime);
        //img_CutScene1_BackgroundImageDown.transform.DoScaleY(0, playTime);
        showSceneSequence2.Restart();
    }
    public void Show3()
    {
        //这里写死y = -720 是因为Canvas设置了以高来匹配屏幕大小，Canvas的height始终为1440（参考分辨率），其他模式记得获取Canvas的高度来设置位置
        img_CutScene1_BackgroundImageTop.rectTransform.anchoredPosition = new Vector2 { x = 0, y = -720 };
        img_CutScene1_BackgroundImageDown.rectTransform.anchoredPosition = new Vector2 { x = 0, y = -720 };
        img_CutScene1_BackgroundImageTop.rectTransform.localScale = Vector3.one;
        img_CutScene1_BackgroundImageDown.rectTransform.localScale = Vector3.one;
        //img_CutScene1_BackgroundImageTop.transform.DoScaleY(0, playTime);
        //img_CutScene1_BackgroundImageDown.transform.DoScaleY(0, playTime);
        showSceneSequence3.Restart();
    }

    private void OnHideSceneComplete()
    {
        EventMgr.Instance.OnEventTrigger(Consts.E_OnHideSceneComplete);
    }

    private void OnShowSceneComplete()
    {
        EventMgr.Instance.OnEventTrigger(Consts.E_OnShowSceneComplete);
    }
}