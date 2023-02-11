using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefendState : PlayerBaseState
{
    public DefendState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _sh.GetCurrentSkillName() == Consts.J_JumpKickLeft
                    || _sh.GetCurrentSkillName() == Consts.J_JumpKickRight, Consts.S_JumpKickEnter);
        AddTargetState(() => !_ih.isDefend && _ac.CurAnimNormalizedTime >= 1f, Consts.S_Idle);
    }

    public override void OnFixedUpdate()
    {
        _mc.OptimizeSlopeMovement();
        _mc.AddFriction(0.08f);
    }
}
