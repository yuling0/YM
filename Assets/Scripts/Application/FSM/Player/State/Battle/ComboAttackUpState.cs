using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttackUpState : PlayerAttackState
{
    public ComboAttackUpState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }
    public override void AnimationEventTrigger()
    {
        _mc.SetVelocityX(_erqieInfo.ComboAttackUpVelocity);
    }
}
