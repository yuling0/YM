using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheFullMoonSlashState : PlayerAttackState
{
    public TheFullMoonSlashState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_FallEnter);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _pfsm.isCanJumpAttack = false;
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.MovementOfSkillInTheAir();
        _fsm.GenerateGhosting();
    }
}
