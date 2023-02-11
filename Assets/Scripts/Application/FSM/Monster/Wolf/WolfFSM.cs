using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfFSM : MonsterFSM
{
    WolfInfo wolfInfo;
    public float yHeight;
    public override void InitFSM()
    {
        base.InitFSM();
        wolfInfo = _info as WolfInfo;

        stateDic.Add(Consts.S_Idle, new WolfIdleState(this, _core, Consts.A_Idle));
        stateDic.Add(Consts.S_ChaseState, new WolfChaseState(this, _core, Consts.A_Move));
        stateDic.Add(Consts.S_MonsterAttackState, new WolfAttackState(this, _core, Consts.A_Idle));
        stateDic.Add(Consts.S_NormalAttack, new WolfNormalAttackState(this, _core, Consts.A_NormalAttack));
        stateDic.Add(Consts.S_ComboAttackForward, new WolfComboAttackState(this, _core, Consts.A_ComboAttackForward));
        stateDic.Add(Consts.S_UpSlash, new WolfUpSlashState(this, _core, Consts.A_UpSlash));
        stateDic.Add(Consts.S_HurtState, new WolfHurtState(this, _core, Consts.A_Hurt));
        ChangeState(Consts.S_Idle);

    }
    protected override void DrawRange()
    {
        DrawUtility.DrawCircle(transform,wolfInfo.ChaseRange, Color.yellow);
        DrawUtility.DrawCircle(transform,wolfInfo.attackRange, Color.red);
        DrawUtility.DrawSector(transform, wolfInfo.detectionRange, Color.green, 60, _mmc.IsFacingRight, yHeight);
    }
}
