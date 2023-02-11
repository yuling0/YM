using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSwordState : PlayerBaseState
{
    bool isBattleState;
    public DrawSwordState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);
    }
    public override void OnEnter()
    {
        isBattleState = !(_fsm as PlayerFSM).isBattleState;

        (_fsm as PlayerFSM).isBattleState = isBattleState;

        _ac.PlayAnim(isBattleState ? Consts.A_DrawSword : Consts.A_RetractSword);
    }
}
