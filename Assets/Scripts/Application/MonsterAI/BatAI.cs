using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

public class BatAI : MonsterAI
{
    SoundHandler soundHandler;
    BatInfo batInfo;
    HitHandler hitHandler;
    Collider2D[] hitCollider;

    protected override void InitTree()
    {
        base.InitTree();
        soundHandler = GetComponentInCore<SoundHandler>();
        hitHandler = GetComponentInCore<HitHandler>();
        batInfo = info as BatInfo;
        hitCollider = new Collider2D[1];
    }
    protected override void InitFunctionMap()
    {
        base.InitFunctionMap();
        functionDic.Add("SprintAttack", (System.Action)SprintAttack);
        functionDic.Add("SprintEnter", (System.Action)SprintEnter);
        functionDic.Add("Hurt", (System.Action)Hurt);
    }
    public override void OnEnableComponent()
    {
        base.OnEnableComponent();
        GlobalClock.Instance.AddTimer(0.6f, -1, PlayBatFlySound);

        //在冲刺状态和受伤状态不播放蝙蝠飞行音效
        blackboard.AddObserver("Sprint", OnValueChanged);
        blackboard.AddObserver("Hurt", OnValueChanged);
    }

    private void OnValueChanged(E_ChangeType arg1, ISharedType arg2)
    {
        GlobalClock.Instance.RemoveTimer(PlayBatFlySound);
    }

    public override void OnDisableComponent()
    {
        base.OnDisableComponent();

        //在冲刺状态和受伤状态不播放蝙蝠飞行音效
        GlobalClock.Instance.RemoveTimer(PlayBatFlySound);
        blackboard.RemoveObserver("Sprint", OnValueChanged);
        blackboard.RemoveObserver("Hurt", OnValueChanged);
    }
    public override void OnUpdateComponent()
    {
        attackTimer += Time.deltaTime;
        
        
        if (isContinuousAttacking)
        {
            if (hitHandler.DetectAttackHit(hitCollider,info.targetMask))
            {
                EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, movementController.IsFacingRight ? 1.5f : -1.5f));
                blackboard.SetTrigger("IsHit");
            }
        }
    }
    /// <summary>
    /// 蝙蝠冲刺前摇
    /// </summary>
    private void SprintEnter()
    {
        movementController.SetVelocityZore();
        movementController.FacingPlayer();
        PlaySound("BatAttackEnter");
    }
    /// <summary>
    /// 冲刺攻击状态
    /// </summary>
    private void SprintAttack()
    {
        movementController.FacingPlayer();
        movementController.SetVelocity(movementController.distance.normalized * batInfo.sprintSpeed);
    }
    protected override void Idle()
    {
        (movementController as BatMovementController).Idle();
    }

    protected override void CheckAttack()
    {
        if (attackTimer >= info.attackInterval && movementController.CheckAttackRange())
        {
            blackboard.SetTrigger("Sprint");
        }
    }
    protected override void ResetAttack()
    {
        base.ResetAttack();
        GlobalClock.Instance.AddTimer(0.6f, -1, PlayBatFlySound);
    }
    protected override void CheckDetectionRange()
    {
        if (movementController.CheckDetectionRange())
        {
            blackboard.SetKey("Chasing", new SharedBool(true));
        }
        else
        {
            blackboard.SetKey("Chasing", new SharedBool(false));
        }
    }
    private void Hurt()
    {
        AttackTimer = 0f;
        movementController.SetVelocityY(-3f);
        isContinuousAttacking = false;
        GlobalClock.Instance.AddTimer(0.6f, -1, PlayBatFlySound);
    }
    
    /// <summary>
    /// 播放蝙蝠飞行音效（因为需要持续播放，冲刺时不播放）
    /// </summary>
    private void PlayBatFlySound()
    {
        PlaySoundParams ps = PlaySoundParams.Create();
        ps.SpatialBlend = 0.8f;
        ps.MinDistance = 3f;
        ps.MaxDistance = 8f;
        ps.AudioRolloffMode = AudioRolloffMode.Linear;
        SoundHandler.PlaySound("BatFly", ps);
    }


    protected override void PlaySound(string soundName)
    {
        PlaySoundParams ps = PlaySoundParams.Create();
        ps.SpatialBlend = 0.8f;
        ps.MinDistance = 3f;
        ps.MaxDistance = 8f;
        ps.AudioRolloffMode = AudioRolloffMode.Linear;
        SoundHandler.PlaySound(soundName, ps);

    }
}
