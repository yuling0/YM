using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WideSlashState : PlayerAttackState
{
    public WideSlashState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }
    public override void AnimationEventTrigger()
    {

        _mc.SetVelocityX(_erqieInfo.WideSlashVelocity);
    }
}
