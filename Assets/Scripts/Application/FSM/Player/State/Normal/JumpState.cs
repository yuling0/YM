using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState
{
    public bool jumpEnterPlayed;
    public JumpState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _sh.CheckSkillIsCompelted(Consts.J_UpSlash) && _pfsm.isBattleState, Consts.S_TheFullMoonSlash);

        AddTargetState(() => _mc.Velocity.y < 0, Consts.S_FallEnter);

        AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        _sh.CheckSkillIsCompelted(Consts.J_NormalAttack), Consts.S_NormalJumpAttack);

        //AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        //_sh.GetCurrentSkillName() == Consts.J_WideSlashRight && _mc.isFacingRight, Consts.S_NormalJumpAttack);

        //AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        //_sh.GetCurrentSkillName() == Consts.J_WideSlashLeft && !_mc.isFacingRight, Consts.S_NormalJumpAttack);

        //AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        //_sh.GetCurrentSkillName() == Consts.J_NormalAttack, Consts.S_NormalJumpAttack);

    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.Jump();
        jumpEnterPlayed = false;
        _pfsm.isCanJumpAttack = true;
    }

    public override void OnUpdate()
    {
        if(!jumpEnterPlayed && _ac.CurAnimNormalizedTime > 0.9f)
        {
            _ac.PlayAnim( !_pfsm.isBattleState ? Consts.A_JumpStay : Consts.A_BattleJumpStay);
            jumpEnterPlayed = true;
        }

        _mc.Jump();

    }
    public override void OnFixedUpdate()
    {
        _mc.Jump();
    }
}
