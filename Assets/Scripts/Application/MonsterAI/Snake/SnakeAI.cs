using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.BehaviorTree;
using Random = UnityEngine.Random;

public class SnakeAI : MonsterAI
{
    SnakeMovementController smc;
    SnakeInfo snakeInfo;
    HitHandler hitHandler;
    Collider2D [] attackResult;

    public bool isIdle;
    public bool isPatrol;
    public int randomNum;
    public int minSecond;
    public int maxSecond;
    public float wideBiteTimer;

    protected override void InitTree()
    {
        base.InitTree();
        smc = GetComponentInCore<SnakeMovementController>();
        snakeInfo = info as SnakeInfo;
        hitHandler = GetComponentInCore<HitHandler>();
        attackResult = new Collider2D[3];

        StartCoroutine(RandomBehavior());
    }
    protected override void InitFunctionMap()
    {
        base.InitFunctionMap();
        functionDic.Add("WideBite", (System.Action)WideBite);
        functionDic.Add("ResetHurt", (System.Action)ResetHurt);
        functionDic.Add("BiteEnter", (System.Action)BiteEnter);
        functionDic.Add("WideBiteBuffer", (System.Action)(() => { smc.SetVelocityX(-0.3f); }));
        functionDic.Add("IsMoving", (Func<bool>)(() => smc.Velocity.x != 0f));

    }

    private IEnumerator RandomBehavior()
    {
        while (true)
        {
            randomNum = Random.Range(1, 4);

            yield return new WaitForSeconds(Random.Range(minSecond, maxSecond));
        }

    }

    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        wideBiteTimer += Time.deltaTime;
        if (isContinuousAttacking)
        {
            if (hitHandler.DetectAttackHit(attackResult, info.targetMask))
            {
                //isHit = true;
                blackboard.SetTrigger("isHit");
                EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, smc.IsFacingRight ? 2 : -2));
            }
        }

        if (randomNum > 2)
        {
            isIdle = true;
            isPatrol = false;
        }
        else
        {
            isIdle = false;
            isPatrol = true;
        }
        blackboard.SetKey("Idle", new SharedBool(isIdle));
        blackboard.SetKey("Patrol", new SharedBool(isPatrol));
    }
    protected override void CheckAttack()
    {

        //if (attackTimer < info.attackInterval) return;

        if (attackTimer >= info.attackInterval && smc.CheckBiteRange() && Mathf.Abs(smc.distance.y) < 0.3f)
        {
            smc.FacingPlayer();
            attackTimer = 0f;
            blackboard.SetTrigger("Bite");

        }
        else if (wideBiteTimer >= snakeInfo.wideBiteInterval && smc.CheckWiteBiteRange() && Mathf.Abs(smc.distance.y) < 0.3f)
        {
            smc.FacingPlayer();
            blackboard.SetTrigger("WideBite");
            wideBiteTimer = 0f;
            //attackTimer = 0f;
        }
    }
    protected override void ResetAttack()
    {
        wideBiteTimer = 0f;
        isContinuousAttacking = false;
    }
    private void ResetHurt()
    {
        isContinuousAttacking = false;
        blackboard.SetKey("Chase", new SharedBool(true));
    }
    private void BiteEnter()
    {
        smc.FacingPlayer();
        smc.SetVelocityZore();
    }
    private void WideBite()
    {
        smc.SetVelocity(smc.IsFacingRight ? snakeInfo.sprintSpeed.x : -snakeInfo.sprintSpeed.x, snakeInfo.sprintSpeed.y);
    }
    protected override void OnceAttackTrigger()
    {
        if (hitHandler.DetectAttackHit(attackResult, info.targetMask))
        {
            //isHit = true;
            //blackboard.SetTrigger("isHit");
            EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, smc.IsFacingRight ? 2 : -2));
            smc.SetVelocityX(-0.5f);
        }
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
