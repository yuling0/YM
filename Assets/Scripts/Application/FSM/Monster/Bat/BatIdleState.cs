using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatIdleState : MonsterBaseState
{
    BatInfo batInfo;

    public BatIdleState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        batInfo = _info as BatInfo;


        AddTargetState(() => _distance.magnitude < batInfo.detectionRange, Consts.S_ChaseState);             //进入追击状态


        AddTargetState(() => (_fsm as BatFSM).canAttack &&
        _distance.magnitude < batInfo.attackRange, Consts.S_SprintEnterState);         //进入攻击状态
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        //Vector2 dir = _distance.normalized;
        //dir.x = 0;
        //_mmc.HorizontalFollow(dir);
        //_mmc.Move();

        //if(_distance.x > 0 && !_mmc.IsFacingRight || _distance.x < 0 && _mmc.IsFacingRight)
        //{
        //    _mmc.Flip();
        //}

    }
}
