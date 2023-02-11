using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : PlayerBaseState
{
    protected HashSet<Collider2D> detectedSet;
    protected bool _isAttacking;
    protected HitHandler _hh;
    private Collider2D[] targetCollider;

    public PlayerAttackState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        _hh = _core.GetComponentInCore<HitHandler>();
        detectedSet = new HashSet<Collider2D>();
        targetCollider = new Collider2D[10];

    }
    public override void OnEnter()
    {
        base.OnEnter();
        _isAttacking = false;
        detectedSet.Clear();
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        if(_isAttacking)
        {
            if (_hh.DetectAttackHit(targetCollider, _erqieInfo.targetMask))
            {
                foreach (var collider in targetCollider)
                {
                    if (collider!= null/* && collider.CompareTag("Monster")*/ && !detectedSet.Contains(collider))
                    {
                        EventMgr.Instance.OnMultiParameterEventTrigger(MonsterBeHitEventArgs.Create(collider.gameObject, 10, _mc.IsFacingRight ? 1.5f : -1.5f));
                        detectedSet.Add(collider);
                        Debug.Log(collider.gameObject.name);
                    }
                }
            }
            
        }
    }
    public void OnceAttackTrigger()
    {
        if (_hh.DetectAttackHit(targetCollider, _erqieInfo.targetMask))
        {
            foreach (var collider in targetCollider)
            {
                if (collider != null /*&& collider.CompareTag("Monster")*/)
                {
                    EventMgr.Instance.OnMultiParameterEventTrigger(MonsterBeHitEventArgs.Create(collider.gameObject, 10, _mc.IsFacingRight ? 1.5f : -1.5f));
                }
                //Debug.Log(collider.name);
            }
        }
    }
    public void ContinuousAttackEnter()
    {
        _isAttacking = true;
        detectedSet.Clear();
    }

    public void ContinuousAttackExit()
    {
        _isAttacking = false;
        
    }
}
