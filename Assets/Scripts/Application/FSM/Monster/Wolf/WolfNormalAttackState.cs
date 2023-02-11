using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfNormalAttackState : MonsterAttackState
{
    public WolfNormalAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1 
        && _mmc.CheckAttackRange() 
        && _mmc.CheckIfPlayerInFrontOfMonster() 
        && Random.Range(0, 2) == 1 , Consts.S_ComboAttackForward);   //随机进入连击状态

        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_ChaseState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
        _mfsm.attackTimer = 0f;
    }
}
