using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMovementController : MovementController
{
    protected Transform _playerTF;      //玩家变换组件
    public Vector3 distance;          //移动方向向量
    protected MonsterInfo _info;        //怪物信息
    PointHandler _ph;                   //巡逻点和出生点管理者
    Vector3 curPatrolPoint;           //当前巡逻点

    public override void Init(Core obj, object userData)
    {
        base.Init(obj, userData);
        _playerTF = GameManager.PlayerTF;
        _info = obj.info as MonsterInfo;
        _ph = obj.GetComponentInCore<PointHandler>();
    }

    public override void OnEnableComponent()
    {
        curPatrolPoint = _ph.GetCurrentPatrolPoint;
    }
    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        distance = _playerTF.position - transform.position;
    }
    /// <summary>
    /// 向玩家方向移动
    /// </summary>
    public override void Move()
    {
        //if (_playerTF.position.x > transform.position.x)        //右移
        //{
        //    SetVelocityX(_info.moveSpeed,false);
        //}
        //else
        //{
        //    SetVelocityX(-_info.moveSpeed,false);        //左移
        //}

        if (Mathf.Abs(distance.x) > _info.optimalRange.y && _playerTF.position.x > transform.position.x)
        {
            SetVelocityX(_info.moveSpeed, false);
        }
        else if (Mathf.Abs(distance.x) > _info.optimalRange.y && _playerTF.position.x < transform.position.x)
        {
            SetVelocityX(-_info.moveSpeed, false);
        }
        else if (Mathf.Abs(distance.x) < _info.optimalRange.x && _playerTF.position.x < transform.position.x)
        {
            SetVelocityX(_info.moveSpeed, false);
        }
        else if (Mathf.Abs(distance.x) < _info.optimalRange.x && _playerTF.position.x > transform.position.x)
        {
            SetVelocityX(-_info.moveSpeed, false);
        }
        if (Velocity.x > 0 && !isFacingRight || Velocity.x < 0 && isFacingRight)
        {
            Flip();
        }
        else if(Velocity.x == 0)
        {
            FacingPlayer();
        }
    }

    public virtual void FacingPlayer()
    {
        if (distance.x > 0 && !isFacingRight || distance.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    /// <summary>
    /// 巡逻
    /// </summary>
    public virtual void Patrol()
    {
        //已到达目标点，更换巡逻点
        if (Mathf.Abs(curPatrolPoint.x - transform.position.x) < 0.1f)
        {
            curPatrolPoint = _ph.GetCurrentPatrolPoint;
        }

        //右移
        if (curPatrolPoint.x > transform.position.x)
        {
            SetVelocityX(_info.moveSpeed,false);
        }
        //左移
        else
        {
            SetVelocityX(-_info.moveSpeed,false);
        }
        //切换方向
        if (isFacingRight && curPatrolPoint.x < transform.position.x || !isFacingRight && curPatrolPoint.x > transform.position.x)
        {
            Flip();
        }
    }
    /// <summary>
    /// 检测玩家是否在侦查范围
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckDetectionRange()
    {
        return CheckInRange(_info.detectionRange);
    }
    /// <summary>
    /// 检测玩家是否在追击范围
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckChaseRange()
    {
        return CheckInRange(_info.ChaseRange);
    }
    /// <summary>
    /// 检测玩家是否在攻击范围
    /// </summary>
    /// <returns></returns>
    public virtual bool CheckAttackRange()
    {
        return CheckInRange(_info.attackRange);
    }

    /// <summary>
    /// 检测是否在目标范围
    /// </summary>
    /// <param name="targetRange"></param>
    /// <returns></returns>
    public bool CheckInRange(float targetRange)
    {
        if (Mathf.Abs(distance.x) <= targetRange)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 检测玩家是否在怪物前方
    /// </summary>
    public virtual bool CheckIfPlayerInFrontOfMonster()
    {
        if (IsFacingRight && distance.x > 0 || !IsFacingRight && distance.x <= 0)
            return true;
        return false;
    }
    public void SetPatrolPos()
    {

    }

}
