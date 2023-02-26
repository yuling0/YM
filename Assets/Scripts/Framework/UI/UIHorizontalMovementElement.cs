
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHorizontalMovementElement : UIComponent
{
    public float endX;      //水平移动量
    public float originX;
    public override void OnInit()
    {
        base.OnInit();
        _showAnim = _rt.DoAnchorPosX(new Vector2(originX, 0),endX, showAnimDuration);
        _hideAnim = _rt.DoAnchorPosX(originX, hideAnimDuration);
    }

}
