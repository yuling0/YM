using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalJumpAttackState : PlayerAttackState
{

    public NormalJumpAttackState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_FallEnter);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _pfsm.isCanJumpAttack = false;
        //_mc.SetVelocityY(0f);
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.MovementOfSkillInTheAir();
    }
}
