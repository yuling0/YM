using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeWideBiteEnter : MonsterBaseState
{
    public SnakeWideBiteEnter(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_WideBiteState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocityZore();
        (_mfsm as SnakeFSM).wideBiteTimer = 0f;
        _mfsm.attackTimer = 0f;
    }
}
