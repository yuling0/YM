using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVerticalMovementElement : UIComponent
{
    public float endY;      //垂直移动量
    public float originY;
    public override void OnInit()
    {
        base.OnInit();
        _showAnim = _rt.DoAnchorPosY(new Vector2(0, originY),endY, showAnimDuration);
        _hideAnim = _rt.DoAnchorPosY(originY, hideAnimDuration);
    }
}
