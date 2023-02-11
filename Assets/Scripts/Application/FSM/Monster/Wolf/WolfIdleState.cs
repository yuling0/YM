using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfIdleState : MonsterBaseState
{
    public WolfIdleState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _mmc.CheckDetectionRange(), Consts.S_ChaseState);
    }

}
