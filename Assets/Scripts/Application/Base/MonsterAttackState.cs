using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterBaseState
{
    protected HashSet<GameObject> detectedSet;
    protected bool _isAttacking;
    protected HitHandler _hh;
    protected bool _isHit;
    protected RaycastHit2D[] raycastHits;

    public MonsterAttackState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        _hh = _core.GetComponentInCore<HitHandler>();
        detectedSet = new HashSet<GameObject>();
        raycastHits = new RaycastHit2D[3];
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
            if (_hh.DetectAttackHit(raycastHits, _info.targetMask , out int hitCount))
            {
                //foreach (var collider in targetCollider)
                //{
                //    if (collider != null && collider.CompareTag("Player") && !detectedSet.Contains(collider))
                //    {
                //        EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                //        detectedSet.Add(collider);
                //        _isHit = true;
                //    }
                //}
                for (int i = 0; i < hitCount; i++)
                {
                    GameObject hitObject = raycastHits[i].transform.gameObject;
                    if (!detectedSet.Contains(hitObject) && hitObject.CompareTag("Player"))
                    {
                        //int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
                        Vector2 hitPoint = Camera.main.WorldToScreenPoint(raycastHits[i].point);
                        EventMgr.Instance.OnMultiParameterEventTrigger
                        (PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                        detectedSet.Add(hitObject);
                        _isHit = true;
                    }
                }
            }
        }
    }
    public virtual void OnceAttackTrigger(int id)
    {
        //if (_hh.DetectAttackHit(targetCollider, _info.targetMask))
        //{
        //    foreach (var collider in targetCollider)
        //    {
        //        if (collider != null && collider.CompareTag("Player"))
        //        {
        //            EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
        //            _isHit = true;
        //            OnceAttackAfter();
        //        }
        //        //Debug.Log(collider.name);
        //    }
        //}
        if (_hh.DetectAttackHit(raycastHits, _info.targetMask, out int hitCount))
        {
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = raycastHits[i].transform.gameObject;
                if (hitObject.CompareTag("Monster"))
                {
                    //int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
                    Vector2 hitPoint = Camera.main.WorldToScreenPoint(raycastHits[i].point);
                    EventMgr.Instance.OnMultiParameterEventTrigger
                    (PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                    _isHit = true;
                    OnceAttackAfter();
                }
            }
        }
    }

    protected virtual void OnceAttackAfter()
    {

    }


    public void ContinuousAttackEnter(int id)
    {
        _isAttacking = true;
    }

    public void ContinuousAttackExit(int id)
    {
        _isAttacking = false;
    }
}
