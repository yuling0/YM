using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardRollState : PlayerBaseState
{
    public ForwardRollState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 0.99, Consts.S_Idle);
    }

    public override void OnEnter()
    {

        base.OnEnter();


    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _fsm.GenerateGhosting();
        _mc.SetVelocityX(_mc.IsFacingRight ? _erqieInfo.ForwardRollVelocity : -_erqieInfo.ForwardRollVelocity, false);
    }

}
