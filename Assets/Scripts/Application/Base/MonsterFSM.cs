using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFSM : BaseFSM
{
    protected MonsterMovementController _mmc;
    protected MonsterInfo _info;
    protected AnimationController _ac;
    public bool canJumpAttack;
    public bool canAttack;
    public float attackTimer;
    public override void InitFSM()
    {
        base.InitFSM();
        _mmc = _core.GetComponentInCore<MonsterMovementController>();
        _info = _core.info as MonsterInfo;
        _ac = _core.GetComponentInCore<AnimationController>();
        attackTimer = _info.attackInterval;
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        CheckAttackTimer();
        DrawRange();
    }

    /// <summary>
    /// 检测是否可以进行攻击
    /// </summary>
    private void CheckAttackTimer()
    {
        canAttack = false;
        attackTimer += Time.deltaTime;
        if (attackTimer > _info.attackInterval) canAttack = true;
    }
    /// <summary>
    /// 绘制范围
    /// </summary>
    protected virtual void DrawRange()
    {

    }

    public override void OnceAttackTrigger()
    {
        (currentState as MonsterAttackState)?.OnceAttackTrigger();
        Debug.Log("OnceAttackTrigger");
    }

    public override void ContinuousAttackEnter()
    {
        (currentState as MonsterAttackState)?.ContinuousAttackEnter();
    }

    public override void ContinuousAttackExit()
    {
        (currentState as MonsterAttackState)?.ContinuousAttackExit();
    }

}
