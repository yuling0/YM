using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    protected HashSet<Collider2D> detectedSet;
    protected bool _isAttacking;
    protected HitHandler _hh;
    protected bool _isHit;
    protected Collider2D[] targetCollider;

    public MonsterAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        _hh = _core.GetComponentInCore<HitHandler>();
        detectedSet = new HashSet<Collider2D>();
        targetCollider = new Collider2D[3];
    }
    public override void OnEnter()
    {
        base.OnEnter();
        detectedSet.Clear();
        _isHit = false;
        _isAttacking = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        DetectContinuousAttack();
    }

    protected virtual void DetectContinuousAttack()
    {
        if (_isAttacking)
        {
            if (_hh.DetectAttackHit(targetCollider, _info.targetMask))
            {
                foreach (var collider in targetCollider)
                {
                    if (collider != null && collider.CompareTag("Player") && !detectedSet.Contains(collider))
                    {
                        EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                        detectedSet.Add(collider);
                        _isHit = true;
                    }
                }
            }
        }
    }
    public virtual void OnceAttackTrigger()
    {
        if (_hh.DetectAttackHit(targetCollider, _info.targetMask))
        {
            foreach (var collider in targetCollider)
            {
                if (collider != null && collider.CompareTag("Player"))
                {
                    EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                    _isHit = true;
                    OnceAttackAfter();
                }
                //Debug.Log(collider.name);
            }
        }
    }

    protected virtual void OnceAttackAfter()
    {

    }


    public void ContinuousAttackEnter()
    {
        _isAttacking = true;
    }

    public void ContinuousAttackExit()
    {
        _isAttacking = false;
    }
}
