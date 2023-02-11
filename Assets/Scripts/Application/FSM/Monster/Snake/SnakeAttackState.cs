using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeAttackState : MonsterBaseState
{
    SnakeFSM _snakeFSM;
    SnakeInfo _si;
    public SnakeAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        _si = core.info as SnakeInfo;
        _snakeFSM = _mfsm as SnakeFSM;
        AddTargetState(() => _distance.y > 0.1f && Mathf.Abs(_distance.x) <= _si.biteRange && _mmc.IsGrounded, Consts.S_JumpEnterState);    //尝试跳跃攻击玩家

        AddTargetState(() => _mfsm.canAttack && _snakeFSM.canWideBite && _mmc.CheckAttackRange() && Mathf.Abs(_distance.y) <= 0.3f
        && Mathf.Abs(_distance.x) <= _si.wideBiteRange && _mmc.CheckIfPlayerInFrontOfMonster(), Consts.S_WideBiteEnterState);    //大范围的咬

        AddTargetState(() => _mfsm.canAttack && _mmc.CheckAttackRange() && Mathf.Abs(_distance.y) <= 0.3f
        && Mathf.Abs(_distance.x) <= _si.biteRange && _mmc.CheckIfPlayerInFrontOfMonster(), Consts.S_BiteState);        //咬

        AddTargetState(() => !_mmc.CheckAttackRange(), Consts.S_ChaseState);


    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.SetVelocityX(0f);
        //_mmc.FacingPlayer();
        if(Mathf.Abs(_distance.x) >= _si.biteRange)
        {
            _mmc.Move();
        }
    }
}
