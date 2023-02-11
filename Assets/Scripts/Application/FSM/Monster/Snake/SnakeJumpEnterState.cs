using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeJumpEnterState : MonsterBaseState
{
    SnakeInfo snakeInfo;
    public SnakeJumpEnterState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        snakeInfo = _info as SnakeInfo;
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_JumpStayState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityY(snakeInfo.jumpForce);
        _mfsm.canJumpAttack = true;
    }
}
