using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : PlayerBaseState
{
    public WalkState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        //AddTargetState(() => _pfsm.isBattleState && _sh.CheckSkillIsCompelted(Consts.J_LeapingSlashRight), Consts.S_LeapingSlash);
        //AddTargetState(() => _pfsm.isBattleState && _sh.CheckSkillIsCompelted(Consts.J_LeapingSlashLeft), Consts.S_LeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && (_sh.CheckSkillIsCompelted(Consts.J_LeapingSlashLeft)
                    || _sh.CheckSkillIsCompelted(Consts.J_LeapingSlashRight)), Consts.S_LeapingSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashLeft && !_mc.IsFacingRight, Consts.S_WheelSlashState);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashRight && _mc.IsFacingRight, Consts.S_WheelSlashState);

        AddTargetState(() => _pfsm.isBattleState && !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft, Consts.S_WideSlash);
        AddTargetState(() => _pfsm.isBattleState && _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashRight, Consts.S_WideSlash);



        AddTargetState(() => _sh.GetCurrentSkillName() == Consts.J_JumpKickLeft
                    || _sh.GetCurrentSkillName() == Consts.J_JumpKickRight, Consts.S_JumpKickEnter);

        AddTargetState(() => _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_ForwardRoll
            || !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_BackwardRoll, Consts.S_ForwardRoll);

        AddTargetState(() => _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_BackwardRoll
                            || !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_ForwardRoll, Consts.S_BackwardRoll);

        AddTargetState(() => !_mc.IsGrounded, Consts.S_FallEnter);

        AddTargetState(() => (_ih.isLeftFlip || _ih.isRightFlip) && Mathf.Abs(_mc.Velocity.x) < 0.01f, Consts.S_FlipState);
        //AddTargetState(() => _mc.Velocity.x > 0 && !_mc.IsFacingRight || _mc.Velocity.x < 0 && _mc.IsFacingRight, Consts.S_FlipState);
        //AddTargetState(() => _mc.IsFacingRight && _ih.leftFlip || !_mc.IsFacingRight && _ih.rightFlip, Consts.S_FlipState);

        //AddTargetState(() => Mathf.Abs(_mc.Velocity.x) < 0.01f && 
        //(_mc.IsFacingRight && !_ih.isRightWalk || !_mc.IsFacingRight && !_ih.isLeftWalk), Consts.S_Idle);
        AddTargetState(() => _ih.AbsH == 0 && _mc.Velocity.sqrMagnitude < 0.05f, Consts.S_Idle);
        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);

        AddTargetState(() => _mc.IsFacingRight && _ih.isRightRunning /*&& _mc.Velocity.x > 0*/
        || !_mc.IsFacingRight && _ih.isLeftRunning /*&& _mc.Velocity.x < 0*/, Consts.S_Run);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetSmooth();
    }
    public override void OnFixedUpdate()
    {
        if (_mc.IsFacingRight && _ih.isRightWalk || !_mc.IsFacingRight && _ih.isLeftWalk)
        {
            _mc.Walk();
        }
        else
        {
            _mc.OptimizeSlopeMovement();
        }
        
    }
    public override void OnExit()
    {
        base.OnExit();
    }

    //TODO:根据地形播放行走音效
    protected override void PlayOtherSound()
    {
        PlaySoundParams ps = PlaySoundParams.Create();
        ps.Volume = 0.1f;
        ps.Pitch = 0.8f;
        sh.PlaySound("OnStone", ps);
    }
}
