using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardRollState : PlayerBaseState
{

    public BackwardRollState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }

    public override void OnEnter()
    {

        base.OnEnter();
    }

    public override void OnFixedUpdate()
    {
        _fsm.GenerateGhosting();
        _mc.SetVelocityX(_mc.IsFacingRight ? _erqieInfo.BackwardRollVelocity : -_erqieInfo.BackwardRollVelocity, false);
    }
}
