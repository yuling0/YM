using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.BehaviorTree;

public class MonsterAI : BehaviorTreeController
{
    protected MonsterInfo info;
    protected HitHandler HitHandler;
    protected SoundHandler SoundHandler;
    protected MonsterMovementController movementController;
    [SerializeField]
    protected float attackTimer = 0f;
    protected bool isContinuousAttacking;

    public float AttackTimer
    {
        get => attackTimer;
        set
        {
            attackTimer = Mathf.Clamp(value, 0, info.attackInterval);
        }
    }
    protected override void InitTree()
    {
        info = _core.info as MonsterInfo;
        HitHandler = GetComponentInCore<HitHandler>();
        SoundHandler = GetComponentInCore<SoundHandler>();
        movementController = GetComponentInCore<MonsterMovementController>();
    }
    protected override void InitFunctionMap()
    {
        functionDic.Add("Idle", (System.Action)Idle);
        functionDic.Add("Patrol", (System.Action)Patrol);
        functionDic.Add("Chasing", (System.Action)Chasing);
        functionDic.Add("CheckAttack", (System.Action)CheckAttack);
        functionDic.Add("CheckChaseRange", (System.Action)CheckChaseRange);
        functionDic.Add("CheckDetectionRange", (System.Action)CheckDetectionRange);
        functionDic.Add("ResetAttack", (System.Action)ResetAttack);
    }

    public override void OnUpdateComponent()
    {
        AttackTimer += Time.deltaTime;
    }
    protected virtual void Idle()
    {
        
    }
    protected virtual void Patrol()
    {
        movementController.Patrol();
    }
    protected virtual void Chasing()
    {
        movementController.Move();
    }
    protected virtual void CheckAttack()
    {

    }
    protected virtual void CheckChaseRange()
    {
        if (!movementController.CheckChaseRange())
        {
            blackboard.SetKey("Chase", new SharedBool(false));
        }
    }

    protected virtual void CheckDetectionRange()
    {
        if (movementController.CheckDetectionRange())
        {
            blackboard.SetKey("Chase", new SharedBool(true));
        }
    }
    public virtual void SetHert()
    {
        blackboard.SetTrigger("Hurt");
    }


    protected virtual void ResetAttack()
    {
        AttackTimer = 0f;
        isContinuousAttacking = false;
    }

    #region 动画事件
    protected virtual void AnimationEventTrigger()
    {
        
    }

    protected virtual void PlaySound(string soundName)
    {

    }

    protected virtual void OnceAttackTrigger()
    {

    }
    protected virtual void ContinuousAttackEnter()
    {
        isContinuousAttacking = true;
    }

    protected virtual void ContinuousAttackExit()
    {
        isContinuousAttacking = false;
    }
    #endregion
}
