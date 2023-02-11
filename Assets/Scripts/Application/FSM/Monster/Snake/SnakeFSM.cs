using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeFSM : MonsterFSM
{
    public float height;
    public float xOffset;
    public SnakeInfo snakeInfo; //测试
    public SnakeMovementController smc;
    public int randomBehaviour;
    public bool canWideBite;
    public float wideBiteTimer;
    public override void InitFSM()
    {
        base.InitFSM();

        snakeInfo = _core.info as SnakeInfo;
        smc = _core.GetComponentInCore<SnakeMovementController>();
        Invoke("RandomNum", 5f);

        stateDic.Add(Consts.S_Idle, new SnakeIdleState(this, _core, Consts.A_Idle));
        stateDic.Add(Consts.S_PatrolState, new SnakePatrolState(this, _core, Consts.A_Move));
        stateDic.Add(Consts.S_ChaseState, new SnakeChaseState(this, _core, Consts.A_Move));
        stateDic.Add(Consts.S_MonsterAttackState, new SnakeAttackState(this, _core, Consts.A_Idle));
        stateDic.Add(Consts.S_BiteState, new SnakeBiteState(this, _core, Consts.A_Bite));
        stateDic.Add(Consts.S_WideBiteState, new SnakeWideBiteState(this, _core, Consts.A_WideBite));
        stateDic.Add(Consts.S_JumpEnterState, new SnakeJumpEnterState(this, _core, Consts.A_JumpEnter));
        stateDic.Add(Consts.S_JumpStayState, new SnakeJumpStayState(this, _core, Consts.A_JumpStay));
        stateDic.Add(Consts.S_FallEnter, new SnakeFallState(this, _core, Consts.A_Fall));
        stateDic.Add(Consts.S_HurtState, new SnakeHurtState(this, _core, Consts.A_Hurt));
        stateDic.Add(Consts.S_WideBiteHitState, new SnakeBiteHitState(this, _core, Consts.A_WideBiteHit));
        stateDic.Add(Consts.S_WideBiteEnterState, new SnakeWideBiteEnter(this, _core, Consts.A_WideBiteEnter));
        ChangeState(Consts.S_Idle);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        canWideBite = false;
        wideBiteTimer += Time.deltaTime;
        if(wideBiteTimer > snakeInfo.wideBiteInterval)
        {
            canWideBite = true;
        }
    }
    private void RandomNum()
    {
        randomBehaviour = Random.Range(0, 2);
        Invoke("RandomNum", 5f);
    }

    protected override void DrawRange()
    {
        DrawUtility.DrawSector(transform,snakeInfo.detectionRange, Color.green, 60 ,_mmc.IsFacingRight, height);
        DrawUtility.DrawCircle(transform, snakeInfo.ChaseRange, Color.yellow);
        DrawUtility.DrawCircle(transform, snakeInfo.attackRange, Color.red);
    }

}
