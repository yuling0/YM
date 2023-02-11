using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatChaseState : MonsterBaseState
{
    BatInfo batInfo;
    public BatChaseState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        batInfo = _info as BatInfo;

        AddTargetState(() => (_fsm as BatFSM).canAttack && 
        Vector2.Distance(_monsterTF.position, _ptf.position) < batInfo.attackRange, Consts.S_SprintEnterState);         //进入攻击状态


        AddTargetState(() => Vector2.Distance(_monsterTF.position, _ptf.position) > batInfo.detectionRange, Consts.S_Idle);                  //闲置
    }
    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        if (Mathf.Abs(_mmc.Velocity.x) <= 0 && (_distance.x > 0 && !_mmc.IsFacingRight || _distance.x < 0 && _mmc.IsFacingRight))
        {
            _mmc.Flip();
        }
        _mmc.Move();
    }
}
