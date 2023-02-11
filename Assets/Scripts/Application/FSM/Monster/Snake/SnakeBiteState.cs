using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBiteState : MonsterAttackState
{
    public SnakeBiteState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _isHit, Consts.S_WideBiteHitState);
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_ChaseState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
        _mfsm.attackTimer = 0f;
    }
}
