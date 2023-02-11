using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackwardLeapingSlashState : PlayerAttackState
{

    public BackwardLeapingSlashState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
        //characterSoundList.Add("Arche0006");
        //characterSoundList.Add("Arche0012");
        //characterSoundList.Add("Arche0013");
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetVelocityY(_erqieInfo.BackwardLeapingSlashVelocity.y);
    }

    public override void OnFixedUpdate()
    {
        _mc.SetVelocityX(_mc.IsFacingRight ? -_erqieInfo.BackwardLeapingSlashVelocity.x : _erqieInfo.BackwardLeapingSlashVelocity.x, false);
        _mc.OptimizeFall();
        _fsm.GenerateGhosting();
    }
}
