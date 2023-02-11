using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourchState : PlayerBaseState
{

    public CourchState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {

        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashLeft && _mc.IsFacingRight, Consts.S_BackwardLeapingSlash);
        AddTargetState(() => _pfsm.isBattleState && _sh.GetCurrentSkillName() == Consts.J_BackwardLeapingSlashRight && !_mc.IsFacingRight, Consts.S_BackwardLeapingSlash);



        AddTargetState(() => !_ih.isCourch && _ac.CurAnimNormalizedTime > 0.9f, Consts.S_Idle);

    }

    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.SetFriction();
    }

    public override void OnUpdate()
    {

        _ac.PlayAnim( !_pfsm.isBattleState ? Consts.A_Crouch : Consts.A_BattleCourch);


    }
    public override void OnExit()
    {
        base.OnExit();
        _mc.SetSmooth();
    }
}
