using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBiteHitState : MonsterBaseState
{
    public SnakeBiteHitState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_ChaseState);
    }
}
