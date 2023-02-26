using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAttackState : PlayerBaseState
{
    protected HashSet<GameObject> detectedSet;
    protected bool _isAttacking;
    protected HitHandler _hh;
    private RaycastHit2D[] hits;
    private PlayerSkillData curSkillData;

    public PlayerAttackState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        _hh = _core.GetComponentInCore<HitHandler>();
        detectedSet = new HashSet<GameObject>();
        hits = new RaycastHit2D[10];

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
            if (_hh.DetectAttackHit(hits, _erqieInfo.targetMask,out int hitCount))
            {
                //foreach (var hit in hits)
                //{
                //    if (collider!= null/* && collider.CompareTag("Monster")*/ && !detectedSet.Contains(collider))
                //    {
                //        int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
                //        Vector2 hitPoint = collider.GetContacts
                //        EventMgr.Instance.OnMultiParameterEventTrigger
                //            (MonsterBeHitEventArgs.Create(collider.gameObject, damage, _mc.IsFacingRight ? curSkillData.knockbackValue : -curSkillData.knockbackValue, curSkillData.knockupVlaue));
                //        detectedSet.Add(collider);
                //        Debug.Log(collider.gameObject.name);
                //    }
                //}
                for (int i = 0; i < hitCount; i++)
                {
                    GameObject hitObject = hits[i].transform.gameObject;
                    if (!detectedSet.Contains(hitObject) && hitObject.CompareTag("Monster"))
                    {
                        int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
                        Vector2 hitPoint = hits[i].point;
                        EventMgr.Instance.OnMultiParameterEventTrigger
                        (MonsterBeHitEventArgs.Create(hitPoint, hitObject, damage, _mc.IsFacingRight ? curSkillData.knockbackValue : -curSkillData.knockbackValue, curSkillData.knockupVlaue));
                        detectedSet.Add(hitObject);
                    }
                    
                }
            }
            
        }
    }
    public void OnceAttackTrigger(int id)
    {
        if (curSkillData == null)
        {
            curSkillData = _pfsm.playerSkillDataContainer.GetPlayerSkillData(id);
        }
        if (_hh.DetectAttackHit(hits, _erqieInfo.targetMask, out int hitCount))
        {
            //foreach (var collider in targetCollider)
            //{
            //    if (collider != null /*&& collider.CompareTag("Monster")*/)
            //    {
            //        int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
            //        EventMgr.Instance.OnMultiParameterEventTrigger
            //            (MonsterBeHitEventArgs.Create(collider.gameObject, damage, _mc.IsFacingRight ? curSkillData.knockbackValue : -curSkillData.knockbackValue, curSkillData.knockupVlaue));
            //    }
            //    //Debug.Log(collider.name);
            //}
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = hits[i].transform.gameObject;
                if(hitObject.CompareTag("Monster"))
                {
                    int damage = Mathf.RoundToInt(_erqieInfo.atk * curSkillData.damagePercentage);
                    Vector2 hitPoint = hits[i].point;
                    EventMgr.Instance.OnMultiParameterEventTrigger
                    (MonsterBeHitEventArgs.Create(hitPoint, hitObject, damage, _mc.IsFacingRight ? curSkillData.knockbackValue : -curSkillData.knockbackValue, curSkillData.knockupVlaue));
                }
            }
        }
    }
    public void ContinuousAttackEnter(int id)
    {
        _isAttacking = true;
        if (curSkillData == null)
        {
            curSkillData = _pfsm.playerSkillDataContainer.GetPlayerSkillData(id);
        }
    }

    public void ContinuousAttackExit(int id)
    {
        _isAttacking = false;
    }
}
