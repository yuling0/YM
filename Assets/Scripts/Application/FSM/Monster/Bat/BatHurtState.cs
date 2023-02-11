using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class BatHurtState : MonsterBaseState
{
    public BatHurtState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1f && _ac.GetCurrentAnimatorStateInfo(0).IsName(Consts.A_Hurt), Consts.S_ChaseState);
    }
    public override void OnEnter()
    {
        base.OnEnter();

    }
}
