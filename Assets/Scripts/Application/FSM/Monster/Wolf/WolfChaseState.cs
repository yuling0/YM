using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfChaseState : MonsterBaseState
{
    public WolfChaseState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {

        AddTargetState(() => _mmc.CheckAttackRange(), Consts.S_MonsterAttackState);         //切换攻击状态

        AddTargetState(() => !_mmc.CheckChaseRange(), Consts.S_Idle);           //脱离追击范围，回到闲置
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.Move();
    }
}
