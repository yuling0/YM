using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFSM : MonsterFSM
{
    [InlineEditor]
    public BatInfo batInfo;
    public override void InitFSM()
    {
        base.InitFSM();
        batInfo = _info as BatInfo;
        attackTimer = batInfo.attackInterval;

        stateDic.Add(Consts.S_Idle, new BatIdleState(this, _core, "Fly"));
        stateDic.Add(Consts.S_ChaseState, new BatChaseState(this, _core, "Fly"));
        stateDic.Add(Consts.S_SprintEnterState, new BatSprintEnterState(this, _core, Consts.A_SprintEnter));
        stateDic.Add(Consts.S_SprintStayState, new BatSprintStayState(this, _core, Consts.A_SprintStay));
        stateDic.Add(Consts.S_HurtState, new BatHurtState(this, _core, Consts.A_Hurt));
        ChangeState(Consts.S_Idle);
    }
    protected override void DrawRange()
    {
        DrawUtility.DrawCircle(transform,_info.detectionRange, Color.yellow);
        DrawUtility.DrawCircle(transform, batInfo.optimalRange.x, Color.green);
        DrawUtility.DrawCircle(transform, batInfo.optimalRange.y, Color.green);
        DrawUtility.DrawCircle(transform, _info.attackRange, Color.red);
    }


}
