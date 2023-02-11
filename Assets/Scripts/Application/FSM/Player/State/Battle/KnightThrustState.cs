using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightThrustState : PlayerAttackState
{
    public KnightThrustState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }
    public override void OnEnter()
    {
        base.OnEnter();

    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.SetVelocityX(_erqieInfo.KnightThrustVelocity);
        _fsm.GenerateGhosting();
    }
}
