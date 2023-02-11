using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BasePanel : MonoBehaviour
{
    protected CanvasGroup _cg;
    protected UIComponent[] _uIComponents;

    //protected Sequence _showPanelSequence;      //  显示面板动画序列
    //protected Sequence _hidePanelSequence;      //  隐藏面板动画序列

    protected TweenerSequence _showPanelSequence;      //  显示面板动画序列
    protected TweenerSequence _hidePanelSequence;      //  隐藏面板动画序列
    private Action OnOpenPanelCallback;         //打开面板回调
    private Action OnHidePanelCallback;         //隐藏面板回调
    private void Awake()
    {
        OnInit();
    }
    /// <summary>
    /// 面板初始化
    /// </summary>
    public virtual void OnInit()
    {
        //_showPanelSequence = DOTween.Sequence();
        //_showPanelSequence.SetAutoKill(false);
        //_showPanelSequence.Pause();
        //_showPanelSequence.OnPlay(() => print($"{this.name}的显示动画被播放"));
        //_showPanelSequence.OnComplete(() => print($"{this.name}的显示动画被播放完成"));

        //_showPanelSequence.SetUpdate(true);             //设置动画不被时间缩放影响
        //_hidePanelSequence = DOTween.Sequence();
        //_hidePanelSequence.SetAutoKill(false);
        //_hidePanelSequence.Pause();
        ////_hidePanelSequence.OnPlay(() => print($"{this.name}的隐藏动画被播放"));
        //_hidePanelSequence.SetUpdate(true);

        //_hidePanelSequence.OnComplete(() => { PoolMgr.Instance.PushObj(Consts.P_UI + this.GetType().Name, this.gameObject); });

        _showPanelSequence = TweenManager.Sequence();
        _showPanelSequence.SetAutoKill(false);
        _showPanelSequence.Pause();
        _showPanelSequence.SetUpdate(true);
        _showPanelSequence.OnComplete(() => print($"{this.name}的显示动画被播放完成"));

        _hidePanelSequence = TweenManager.Sequence();
        _hidePanelSequence.SetAutoKill(false);
        _hidePanelSequence.Pause();
        _hidePanelSequence.SetUpdate(true);
        _hidePanelSequence.OnComplete(() => PoolMgr.Instance.PushObj(Consts.P_UI + this.GetType().Name, this.gameObject));
        _hidePanelSequence.OnComplete(() => print($"{this.name}的隐藏动画被播放完成"));

        _uIComponents = GetControls<UIComponent>();
        foreach (var uiControl in _uIComponents)
        {
            uiControl.OnInit();
            if (uiControl.ShowAnim != null)
            {
                _showPanelSequence.Join(uiControl.ShowAnim);
            }
            if (uiControl.HideAnim != null)
            {
                _hidePanelSequence.Join(uiControl.HideAnim);
            }
        }

    }

    /// <summary>
    /// 面板打开
    /// </summary>
    public virtual void OnOpen()
    {
        _cg = gameObject.GetOrAddComponent<CanvasGroup>();
        _cg.interactable = true;
        (transform as RectTransform).offsetMin = Vector2.zero;
        (transform as RectTransform).offsetMax = Vector2.zero;

        if (_uIComponents != null)
        {
            foreach (var uiControl in _uIComponents)
            {
                uiControl.OnShow();
            }
        }
        _showPanelSequence?.Restart();
        OnOpenPanelCallback?.Invoke();
    }

    /// <summary>
    /// 面板显示（被遮挡之后）
    /// </summary>
    public virtual void OnReveal()
    {
        _cg.interactable = true;
        if (_uIComponents != null)
        {
            foreach (var uiControl in _uIComponents)
            {
                uiControl.OnReveal();
            }
        }
    }
    /// <summary>
    /// 面板更新
    /// </summary>
    public virtual void OnUpdate()
    {
        if (_uIComponents != null)
        {
            foreach (var uiControl in _uIComponents)
            {
                uiControl.OnUpdate();
            }
        }
    }
    /// <summary>
    /// 面板被遮挡
    /// </summary>
    public virtual void OnCover()
    {
        _cg.interactable = false;

        if (_uIComponents != null)
        {
            foreach (var uiControl in _uIComponents)
            {
                uiControl.OnCover();
            }
        }
    }

    /// <summary>
    /// 面板隐藏
    /// </summary>
    public virtual void OnHide()
    {
        _cg.interactable = false;

        if (_uIComponents != null)
        {
            foreach (var uiControl in _uIComponents)
            {
                uiControl.OnHide();
            }
        }
        _hidePanelSequence?.Restart();
        OnHidePanelCallback?.Invoke();
        Clear();
    }



    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        return transform.GetControl<T>(controlName);
    }

    protected T[] GetControls<T>() where T : UIBehaviour
    {
        T[] cs = transform.GetComponentsInChildren<T>();
        return cs;
    }


    public virtual void Clear()
    {
        OnOpenPanelCallback = null;
        OnHidePanelCallback = null;
    }
}
