using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipState : PlayerBaseState
{
    bool canFlip;
    float delayTime = 0.10f;
    float timer = 0f;
    public FlipState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashLeft && _mc.IsFacingRight, Consts.S_BackwardLeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashRight && !_mc.IsFacingRight, Consts.S_BackwardLeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft && _mc.IsFacingRight, Consts.S_SnapThrust);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_WideSlashRight && !_mc.IsFacingRight, Consts.S_SnapThrust);
        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);
        AddTargetState(() => _ih.isCourch, Consts.S_Courch);
        //AddTargetState(() => _pfsm.isBattleState && _ih.isLeftThrust && _mc.IsFacingRight, Consts.S_SnapThrust);
        //AddTargetState(() => _pfsm.isBattleState && _ih.isRightThrust && !_mc.IsFacingRight, Consts.S_SnapThrust);
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1f && 
        (_ac.GetCurrentAnimatorStateInfo(0).IsName(Consts.A_TurnLeft)|| _ac.GetCurrentAnimatorStateInfo(0).IsName(Consts.A_TurnRight)), Consts.S_Idle);
    }
    public override void OnEnter()
    {
        timer = 0f;
        _mc.SetVelocityZore();
        canFlip = true;
        //_ac.PlayAnim(_mc.IsFacingRight ? Consts.A_TurnLeft : Consts.A_TurnRight);
        //_mc.Flip();
        //_mc.SetFriction();
    }

    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= delayTime && canFlip)
        {
            _ac.PlayAnim(_mc.IsFacingRight ? Consts.A_TurnLeft : Consts.A_TurnRight);
            _mc.Flip();

            canFlip = false;
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        _mc.SetSmooth();
    }
}
