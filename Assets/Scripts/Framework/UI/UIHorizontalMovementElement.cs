
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHorizontalMovementElement : UIComponent
{
    public float _hOffset;      //水平移动量
    public float orignX;
    public override void OnInit()
    {
        base.OnInit();
        orignX = _rt.anchoredPosition.x;
        Debug.Log(_rt.anchoredPosition);
        _showAnim = _rt.DoAnchorPosX(orignX, showAnimDuration);
        _hideAnim = _rt.DoAnchorPosX(orignX + _hOffset, hideAnimDuration);
    }

}
