using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapThrustState : PlayerAttackState
{
    public SnapThrustState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_FlipState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.Flip();
    }
}
