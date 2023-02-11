using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVerticalMovementElement : UIComponent
{
    public float _vOffset;      //水平移动量
    public float orignY;
    public override void OnInit()
    {
        base.OnInit();
        orignY = _rt.anchoredPosition.y;
        _showAnim = _rt.DoAnchorPosY(orignY, showAnimDuration);
        _hideAnim = _rt.DoAnchorPosY(orignY + _vOffset, hideAnimDuration);
    }
}
