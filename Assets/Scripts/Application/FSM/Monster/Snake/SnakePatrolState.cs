using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePatrolState : MonsterBaseState
{
    public SnakePatrolState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _mmc.CheckDetectionRange(), Consts.S_ChaseState);

        AddTargetState(() => (_fsm as SnakeFSM).randomBehaviour == 0, Consts.S_Idle);
    }


    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.Patrol();
    }
}
