using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : PlayerBaseState
{
    public RunState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_Thrust, Consts.S_KnightThrust);

        AddTargetState(() => _pfsm.isBattleState && _ih.isUpAttack, Consts.S_UpSlash);

        AddTargetState(() => _pfsm.isBattleState && !_mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft, Consts.S_WideSlash);
        AddTargetState(() => _pfsm.isBattleState && _mc.IsFacingRight && _sh.GetCurrentSkillName() == Consts.J_WideSlashRight, Consts.S_WideSlash);

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_NormalAttack, Consts.S_NormalAttack);


        AddTargetState(() => (_ih.isLeftFlip || _ih.isRightFlip) && Mathf.Abs(_mc.Velocity.x) < 0.01f, Consts.S_FlipState);
        //AddTargetState(() => _mc.Velocity.x > 0 && !_mc.IsFacingRight || _mc.Velocity.x < 0 && _mc.IsFacingRight, Consts.S_FlipState);
        //AddTargetState(() => _mc.IsFacingRight && _ih.leftFlip || !_mc.IsFacingRight && _ih.rightFlip, Consts.S_FlipState);

        AddTargetState(() => !_mc.IsGrounded, Consts.S_FallEnter);

        AddTargetState(() => _sh.GetCurrentSkillName() == Consts.J_Up, Consts.S_Defend);

        AddTargetState(() => _mc.Velocity.magnitude <= _erqieInfo.walkSpeed, Consts.S_Walk);

        //AddTargetState(() => _mc.GetVelocity.x == 0f, Consts.S_Idle);

        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetSmooth();
    }
    public override void OnFixedUpdate()
    {
        if (_mc.IsFacingRight && _ih.isRightRunning || !_mc.IsFacingRight && _ih.isLeftRunning)
        {
            _mc.Run();
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

}
