using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfAttackState : MonsterBaseState
{
    public WolfAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _mfsm.canAttack && _mmc.CheckAttackRange() && Mathf.Abs(_distance.y) > 0.3f
                        && _mmc.CheckIfPlayerInFrontOfMonster(), Consts.S_UpSlash);         //上斩

        AddTargetState(() => _mfsm.canAttack && _mmc.CheckAttackRange() && Mathf.Abs(_distance.y) < 0.3f, Consts.S_NormalAttack);   //普通攻击

        AddTargetState(() => !_mmc.CheckAttackRange(), Consts.S_ChaseState);        // 不在攻击范围，切换追击
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
    }
}
