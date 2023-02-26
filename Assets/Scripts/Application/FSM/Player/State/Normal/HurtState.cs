using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtState : PlayerBaseState
{
    public HurtState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1 && 
        ( _pfsm.isBattleState && _ac.GetCurrentAnimatorStateInfo(0).IsName(Consts.A_BattleHurt) ||
        !_pfsm.isBattleState && _ac.GetCurrentAnimatorStateInfo(0).IsName(Consts.A_Hurt)), Consts.S_Idle);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mc.SetVelocityZore();
        _pfsm.skillBufferSpeed = 0f;
    }
}
