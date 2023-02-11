using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfComboAttackState : MonsterAttackState
{
    WolfInfo wolfInfo;
    public WolfComboAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        wolfInfo = _info as WolfInfo;
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_ChaseState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
    }
    public override void AnimationEventTrigger()
    {
        _mmc.SetVelocityX(wolfInfo.comboAttackVelocity);
    }
}
