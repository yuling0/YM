using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BufferState : PlayerBaseState
{
    public BufferState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _sh.GetCurrentSkillName() == Consts.J_JumpKickLeft
                    || _sh.GetCurrentSkillName() == Consts.J_JumpKickRight, Consts.S_JumpKickEnter);

        AddTargetState(() => _pfsm.isBattleState && (_sh.GetCurrentSkillName() == Consts.J_LeapingSlashLeft
                            || _sh.GetCurrentSkillName() == Consts.J_LeapingSlashRight), Consts.S_LeapingSlash);
        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);

        //AddTargetState(() => (_ih.isLeftFlip || _ih.isRightFlip) && Mathf.Abs(_mc.Velocity.x) < 0.01f, Consts.S_FlipState);
        //AddTargetState(() => _mc.Velocity.x > 0 && !_mc.IsFacingRight || _mc.Velocity.x < 0 && _mc.IsFacingRight, Consts.S_FlipState);

        AddTargetState(() => _ih.isDefend, Consts.S_Defend);

        //AddTargetState(() => !_mc.IsFacingRight && _ih.isLeftPress || _mc.IsFacingRight && _ih.isRightPress, Consts.S_Walk);
        //AddTargetState(() => _ih.AbsH >0, Consts.S_Walk);

        AddTargetState(() => _mc.Velocity.sqrMagnitude < 0.01f, Consts.S_Idle);

    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetSmooth();
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.OptimizeSlopeMovement();
        //if (_ih.isLeftPress || _ih.isRightPress)
        //{
        //    _mc.AddFriction(0.15f);
        //}
    }
}
