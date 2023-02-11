using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UIComponent : UIBehaviour
{
    //protected DG.Tweening.Tween _showAnim;
    //protected DG.Tweening.Tween _hideAnim;
    protected Tween _showAnim;
    protected Tween _hideAnim;
    public float showAnimDuration;
    public float hideAnimDuration;
    protected RectTransform _rt;
    public Tween ShowAnim { get => _showAnim; }
    public Tween HideAnim { get => _hideAnim; }

    public virtual void OnInit()
    {
        _rt = transform as RectTransform;
    }

    public virtual void OnShow()
    {

    }

    public virtual void OnUpdate()
    {

    }
    public virtual void OnReveal()
    {

    }
    public virtual void OnCover()
    {

    }
    public virtual void OnHide()
    {

    }
}
