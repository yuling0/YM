using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeIdleState : MonsterBaseState
{
    public SnakeIdleState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => (_mmc as SnakeMovementController).CheckDetectionRange(), Consts.S_ChaseState);

        AddTargetState(() => (_fsm as SnakeFSM).randomBehaviour == 1, Consts.S_PatrolState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }
}
