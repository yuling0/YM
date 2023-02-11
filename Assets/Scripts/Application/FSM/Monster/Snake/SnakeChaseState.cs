using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeChaseState : MonsterBaseState
{
    SnakeInfo _si;
    public SnakeChaseState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        _si = core.info as SnakeInfo;

        AddTargetState(() => _mmc.CheckAttackRange(), Consts.S_MonsterAttackState);

        AddTargetState(() => !(_mmc as SnakeMovementController).CheckChaseRange(), Consts.S_Idle);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _mmc.Move();
    }
}
