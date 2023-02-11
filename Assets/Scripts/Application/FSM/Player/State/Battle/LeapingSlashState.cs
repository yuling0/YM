using YMFramework.BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LeapingSlashState : PlayerAttackState
{
    public LeapingSlashState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_FallEnter);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.SetVelocity(_mc.IsFacingRight ? _erqieInfo.LeapingSlashVelocity.x : - _erqieInfo.LeapingSlashVelocity.x ,
        //     _erqieInfo.LeapingSlashVelocity.y);
        _mc.SetVelocityY(_erqieInfo.LeapingSlashVelocity.y);

        //GlobalClock.Instance.AddTimer(0.02f, -1,( _fsm as PlayerFSM).GenerateGhosting1);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.OptimizeFall();
        _mc.SetVelocityX(_erqieInfo.LeapingSlashVelocity.x);
        _fsm.GenerateGhosting();
    }
    public override void OnExit()
    {
        base.OnExit();
        _pfsm.skillBufferSpeed = _erqieInfo.LeapingSlashBufferVelocity;
        //_mc.SetVelocityX(_erqieInfo.LeapingSlashBufferVelocity);
        //GlobalClock.Instance.RemoveTimer((_fsm as PlayerFSM).GenerateGhosting1);
    }
}
