using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatMovementController : MonsterMovementController
{
    protected BatInfo batInfo;
    public float hMultiplier = 0.3f;
    public float sinMultiplier = 0.3f;
    protected Transform batTF;
    private PointHandler ph;
    public override void Init(Core obj, object userData)
    {
        base.Init(obj, userData);
        batInfo = _core.info as BatInfo;
        batTF = transform;
        ph = GetComponentInCore<PointHandler>();
    }

    public override void Move()
    {
        
        // 如果蝙蝠不在范围，则向玩家靠近
        if(Mathf.Abs(distance.x) > batInfo.attackRange)
        {
            HorizontalFollow(); //尝试Sin曲线移动

            if(-distance.y > batInfo.optimalHight.y) //不在最适高度内
            {
                SetVelocityY(-batInfo.moveSpeed);
            }

            if (-distance.y < batInfo.optimalHight.x) // 低于最适高度，向上移动
            {
                SetVelocityY(batInfo.moveSpeed);
            }
            SetVelocityXAndFlip(distance.x > 0 ? batInfo.moveSpeed : -batInfo.moveSpeed , true); //根据方位水平移动
        }
        // 在攻击范围内
        else if (Mathf.Abs(distance.x) <= batInfo.attackRange)  
        {
            HorizontalFollow();

            if(Mathf.Abs(distance.x) < batInfo.optimalRange.x)
            {
                SetVelocityXAndFlip(distance.x > 0 ? -batInfo.moveSpeed : batInfo.moveSpeed, true); //根据方位水平移动
            }
            else
            {
                FacingPlayer();
                SetVelocityX(0f);
            }

            if (-distance.y > batInfo.optimalHight.y) //不在最适高度内
            {
                SetVelocityY(-batInfo.moveSpeed);
            }
            if (-distance.y < batInfo.optimalHight.x)
            {
                SetVelocityY(batInfo.moveSpeed);
            }
        }

    }
    //public override void FacingPlayer()
    //{
    //    if (isFacingRight && _playerTF.position.x < transform.position.x || !isFacingRight && _playerTF.position.x > transform.position.x)
    //    {
    //        Flip();
    //    }
    //}

    public void Idle()
    {

        HorizontalFollow();
        if (Mathf.Abs(ph.GetBirthPoint.x - batTF.position.x) > batInfo.patrolRange)
        {
            if (ph.GetBirthPoint.x < batTF.position.x)
            {
                SetVelocityXAndFlip(-batInfo.moveSpeed, true);
            }

            else if (ph.GetBirthPoint.x > batTF.position.x)
            {
                SetVelocityXAndFlip(batInfo.moveSpeed, true);
            }
        }

        if (batTF.position.y < ph.GetBirthPoint.y)
        {
            SetVelocityY(batInfo.moveSpeed);
        }
        else if (batTF.position.y > ph.GetBirthPoint.y + batInfo.patrolRange)
        {
            SetVelocityY(-batInfo.moveSpeed);
        }

    }
    //上升高度
    public void RiseHight(Vector2 dir)
    {
        SetVelocity(dir.x * batInfo.moveSpeed, batInfo.moveSpeed);
    }

    /// <summary>
    /// 飞行怪物水平移动（sin曲线移动）
    /// </summary>
    public  void HorizontalFollow()
    {
        SetVelocityY(Mathf.Sin(Time.time * sinMultiplier) * hMultiplier);
    }


}
