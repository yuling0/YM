using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFSM : BaseFSM
{
    public PlayerSkillDataContainer playerSkillDataContainer;
    public bool isCanJumpAttack;
    public float skillBufferSpeed;      //某些技能使用后的产生缓冲速度
    public float bufferSpeed;           //行走或冲刺过后缓冲速度（刹车的速度）
    public float ghostingIntervalTime;
    public float timer;
    public override void InitFSM()
    {
        base.InitFSM();
        playerSkillDataContainer = BinaryDataManager.Instance.GetContainer<PlayerSkillDataContainer>();
        stateDic.Add(Consts.S_Idle, new IdleState(this, _core, Consts.A_Idle, Consts.A_BattleIdle));
        stateDic.Add(Consts.S_Walk, new WalkState(this, _core, Consts.A_Walk, Consts.A_BattleWalk));
        stateDic.Add(Consts.S_Run, new RunState(this, _core, Consts.A_Run, Consts.A_BattleRun));
        stateDic.Add(Consts.S_Jump, new JumpState(this, _core, Consts.A_JumpEnter, Consts.A_BattleJumpEnter));
        stateDic.Add(Consts.S_FallEnter, new FallEnterState(this, _core, Consts.A_FallEnter, Consts.A_BattleFallEnter));
        stateDic.Add(Consts.S_FallStay, new FallStayState(this, _core, Consts.A_FallStay, Consts.A_BattleFallStay));
        stateDic.Add(Consts.S_FallExit, new FallExitState(this, _core, Consts.A_FallExit, Consts.A_BattleFallExit));
        stateDic.Add(Consts.S_ForwardRoll, new ForwardRollState(this, _core, Consts.A_ForwardRoll, Consts.A_BattleForwardRoll));
        stateDic.Add(Consts.S_BackwardRoll, new BackwardRollState(this, _core, Consts.A_BackwardRoll, Consts.A_BattleBackwardRoll));
        stateDic.Add(Consts.S_Courch, new CourchState(this, _core, Consts.A_CrouchEnterAndExit, Consts.A_BattleCourchEnter));
        stateDic.Add(Consts.S_JumpKickEnter, new JumpKickEnterState(this, _core, Consts.A_JumpKickEnter, Consts.A_JumpKickEnter));
        stateDic.Add(Consts.S_JumpKickStay, new JumpKickStayState(this, _core, Consts.A_JumpKickStay, Consts.A_JumpKickStay));
        stateDic.Add(Consts.S_JumpKickNoHit, new JumpKickNoHitState(this, _core, Consts.A_JumpKickExitNoHit, Consts.A_JumpKickExitNoHit));
        stateDic.Add(Consts.S_JumpKickHit, new JumpKickHitState(this, _core, Consts.A_JumpKickExitHit, Consts.A_JumpKickExitHit));
        stateDic.Add(Consts.S_FlipState, new FlipState(this, _core, Consts.A_TurnLeft));
        stateDic.Add(Consts.S_Defend, new DefendState(this, _core, Consts.A_Defend, Consts.A_BattleDefend));
        stateDic.Add(Consts.S_DrawSword, new DrawSwordState(this, _core, Consts.A_DrawSword));
        stateDic.Add(Consts.S_NormalAttack, new NormalAttackState(this, _core, Consts.A_NormalAttack, Consts.A_NormalAttack));
        stateDic.Add(Consts.S_NormalJumpAttack, new NormalJumpAttackState(this, _core, Consts.A_NormalJumpAttack, Consts.A_NormalJumpAttack));
        stateDic.Add(Consts.S_KnightThrust, new KnightThrustState(this, _core, Consts.A_KnightThrust, Consts.A_KnightThrust));
        stateDic.Add(Consts.S_LeapingSlash, new LeapingSlashState(this, _core, Consts.A_LeapingSlash, Consts.A_LeapingSlash));
        stateDic.Add(Consts.S_Thrust, new ThrustState(this, _core, Consts.A_Thrust, Consts.A_Thrust));
        stateDic.Add(Consts.S_SnapThrust, new SnapThrustState(this, _core, Consts.A_SnapThrust, Consts.A_SnapThrust));
        stateDic.Add(Consts.S_WideSlash, new WideSlashState(this, _core, Consts.A_WideSlash, Consts.A_WideSlash));
        stateDic.Add(Consts.S_Buffer, new BufferState(this, _core, Consts.A_Buffer, Consts.A_BattleBuffer));
        stateDic.Add(Consts.S_UpSlash, new UpSlashState(this, _core, Consts.A_UpSlash, Consts.A_UpSlash));
        stateDic.Add(Consts.S_ComboAttackForward, new ComboAttackForwardState(this, _core, Consts.A_ComboAttackForward, Consts.A_ComboAttackForward));
        stateDic.Add(Consts.S_ComboAttackUp, new ComboAttackUpState(this, _core, Consts.A_ComboAttackUp, Consts.A_ComboAttackUp));
        stateDic.Add(Consts.S_BackwardLeapingSlash, new BackwardLeapingSlashState(this, _core, Consts.A_BackwardLeapingSlash, Consts.A_BackwardLeapingSlash));
        stateDic.Add(Consts.S_TheFullMoonSlash, new TheFullMoonSlashState(this, _core, Consts.A_TheFullMoonSlash, Consts.A_TheFullMoonSlash));
        stateDic.Add(Consts.S_DownStabEnterState, new DownStabEnterState(this, _core, Consts.A_DownStabEnter, Consts.A_DownStabEnter));
        stateDic.Add(Consts.S_DownStabStayState, new DownStabStayState(this, _core, Consts.A_DownStabStay, Consts.A_DownStabStay));
        stateDic.Add(Consts.S_DownStabExitState, new DownStabExitState(this, _core, Consts.A_DownStabExit, Consts.A_DownStabExit));
        stateDic.Add(Consts.S_WheelSlashState, new WheelSlashState(this, _core, Consts.A_WheelSlash, Consts.A_WheelSlash));
        stateDic.Add(Consts.S_HurtState, new HurtState(this, _core, Consts.A_Hurt, Consts.A_BattleHurt));
    }
    public override void OnEnableComponent()
    {
        ChangeState(Consts.S_Idle);
    }
    public override void OnceAttackTrigger(int id)
    {
        (currentState as PlayerAttackState)?.OnceAttackTrigger(id);
    }

    public override void ContinuousAttackEnter(int id)
    {
        (currentState as PlayerAttackState)?.ContinuousAttackEnter(id);
    }

    public override void ContinuousAttackExit(int id)
    {
        (currentState as PlayerAttackState)?.ContinuousAttackExit(id);
    }

    //TODO:使用Clock计时
    //public override void GenerateGhosting()
    //{
    //    timer -= Time.deltaTime;

    //    if (timer <= 0f)
    //    {
    //        PoolMgr.Instance.PopObjAsync<GameObject>(Consts.P_Ghosting,
    //            (obj) =>
    //            {
    //                obj.GetComponent<Ghosting>().Enable(spriteRenderer);
    //            });
    //        timer = ghostingIntervalTime;
    //    }


    //}

    //public void GenerateGhosting1()
    //{
    //    PoolMgr.Instance.PopObjAsync<GameObject>(Consts.P_Ghosting,
    //        (obj) =>
    //        {
    //            obj.GetComponent<Ghosting>().Enable(spriteRenderer);
    //        });


    //}
}
