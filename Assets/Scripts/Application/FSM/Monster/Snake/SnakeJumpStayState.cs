using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeJumpStayState : MonsterBaseState
{
    SnakeInfo snakeInfo;
    public SnakeJumpStayState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        snakeInfo = _info as SnakeInfo;
        AddTargetState(() => _mfsm.canAttack && Mathf.Abs(_distance.x) <= snakeInfo.biteRange && Mathf.Abs(_distance.y) <= 0.4f, Consts.S_BiteState);
        AddTargetState(() => _mmc.Velocity.y <= 0, Consts.S_FallEnter);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mfsm.canJumpAttack = false;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.Move();
    }
}
