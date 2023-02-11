using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownStabStayState : PlayerAttackState
{
    public DownStabStayState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _mc.IsGrounded, Consts.S_DownStabExitState);
    }

    public override void OnFixedUpdate()
    {
        _mc.Move();
        _mc.OptimizeFall();
    }
}
