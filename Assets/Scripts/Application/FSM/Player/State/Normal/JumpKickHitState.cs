using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKickHitState : PlayerBaseState
{
    Vector2 forword;
    Vector2 backword;
    public JumpKickHitState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        forword = new Vector2(1.5f, 3);
        backword = new Vector2(-1.5f, 3);
        AddTargetState(() => _mc.IsGrounded && _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetVelocity(_mc.IsFacingRight ? backword : forword);
    }

    public override void OnFixedUpdate()
    {
        _mc.OptimizeFall();
    }
}
