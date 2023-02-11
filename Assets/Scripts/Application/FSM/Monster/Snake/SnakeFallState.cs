using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFallState : MonsterBaseState
{
    SnakeInfo snakeInfo;
    public SnakeFallState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        snakeInfo = _info as SnakeInfo;
        AddTargetState(() => _mfsm.canAttack && Mathf.Abs(_distance.x) <= snakeInfo.biteRange && Mathf.Abs(_distance.y) <= 0.2f, Consts.S_BiteState);
        AddTargetState(() => _mmc.IsGrounded, Consts.S_ChaseState);

    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.OptimizeFall();
    }
}
