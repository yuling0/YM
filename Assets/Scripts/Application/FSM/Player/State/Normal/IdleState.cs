using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : PlayerBaseState
{
    public IdleState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ih.isDrawSwordPress, Consts.S_DrawSword);


        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashLeft && _mc.IsFacingRight, Consts.S_BackwardLeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashRight && !_mc.IsFacingRight, Consts.S_BackwardLeapingSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.CheckSkillIsCompelted(Consts.J_BackwardLeapingSlashLeft) && _mc.IsFacingRight, Consts.S_BackwardLeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && _sh.CheckSkillIsCompelted(Consts.J_BackwardLeapingSlashRight) && !_mc.IsFacingRight, Consts.S_BackwardLeapingSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashLeft && !_mc.IsFacingRight, Consts.S_WheelSlashState);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashRight && _mc.IsFacingRight, Consts.S_WheelSlashState);

        AddTargetState(() => _sh.GetCurrentSkillName() == Consts.J_JumpKickLeft
                            || _sh.GetCurrentSkillName() == Consts.J_JumpKickRight, Consts.S_JumpKickEnter);

        AddTargetState(() => _pfsm.isBattleState && (_sh.CheckSkillIsCompelted(Consts.J_LeapingSlashLeft) 
                            || _sh.CheckSkillIsCompelted(Consts.J_LeapingSlashRight)), Consts.S_LeapingSlash);

        AddTargetState(() => _pfsm.isBattleState && (_sh.GetCurrentSkillName() == Consts.J_LeapingSlashLeft
                        || _sh.GetCurrentSkillName() == Consts.J_LeapingSlashRight), Consts.S_LeapingSlash);

        //AddTargetState(() => _pfsm.isBattleState && (_sh.CheckSkillIsCompelted( Consts.J_LeapingSlashLeft)
        //            || _sh.CheckSkillIsCompelted(Consts.J_LeapingSlashRight)), Consts.S_LeapingSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_UpSlash, Consts.S_UpSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft && _mc.IsFacingRight, Consts.S_SnapThrust);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_WideSlashRight && !_mc.IsFacingRight, Consts.S_SnapThrust);

        AddTargetState(() => _pfsm.isBattleState && !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft ,Consts.S_WideSlash);
        AddTargetState(() => _pfsm.isBattleState && _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashRight , Consts.S_WideSlash);




        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_Thrust, Consts.S_Thrust);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_NormalAttack, Consts.S_NormalAttack);

        AddTargetState(() => _ih.isCourch, Consts.S_Courch);

        AddTargetState(() => _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_ForwardRoll
                    || !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_BackwardRoll, Consts.S_ForwardRoll);

        AddTargetState(() => _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_BackwardRoll
                            || !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_ForwardRoll, Consts.S_BackwardRoll);

        AddTargetState(() => (_ih.isLeftFlip || _ih.isRightFlip) && Mathf.Abs(_mc.Velocity.x) < 0.01f, Consts.S_FlipState);

        AddTargetState(() => _ih.isDefend, Consts.S_Defend);
        AddTargetState(() => !_mc.IsGrounded, Consts.S_FallEnter);
        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);

        //AddTargetState(() => _mc.IsGrounded && _mc.Velocity.x != 0, Consts.S_Buffer);

        AddTargetState(() => _mc.IsFacingRight && _ih.isRightRunning || !_mc.IsFacingRight && _ih.isLeftRunning, Consts.S_Run);

        AddTargetState(() => !_mc.IsFacingRight && _ih.isLeftWalk || _mc.IsFacingRight && _ih.isRightWalk, Consts.S_Walk);
        //AddTargetState(() => _mc.Velocity.sqrMagnitude > 0, Consts.S_Walk);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetVelocityZore();
        //_mc.SetFriction();
        
    }

    public override void OnUpdate()
    {
        _mc.SetVelocityZore();
    }

    public override void OnExit()
    {
        base.OnExit();
        _mc.SetSmooth();
    }
}
