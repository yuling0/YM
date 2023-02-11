using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindow : UIComponent
{
    public override void OnInit()
    {
        base.OnInit();
        _rt.localScale = Vector3.zero;
        _showAnim = _rt.DoScale(Vector3.one, showAnimDuration);
        _hideAnim = _rt.DoScale(Vector3.zero, hideAnimDuration);
    }

}
