using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKickNoHitState : PlayerBaseState
{
    public JumpKickNoHitState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_FallEnter);
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
    public override void OnExit()
    {
        base.OnExit();
        _pfsm.skillBufferSpeed = _erqieInfo.JumpKickBufferVelocity;
        //_mc.SetVelocityX(_erqieInfo.JumpKickBufferVelocity);
    }
}
