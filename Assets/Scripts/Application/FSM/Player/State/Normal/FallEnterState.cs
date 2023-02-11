using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallEnterState : PlayerBaseState
{
    public FallEnterState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _mc.IsGrounded, Consts.S_FallExit);
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1f, Consts.S_FallStay);

        AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        _sh.CheckSkillIsCompelted(Consts.J_NormalAttack), Consts.S_NormalJumpAttack);
    }

}
