using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSprintStayState : MonsterAttackState
{
    private float timer;
    private float duation = 0.8f;
    public BatSprintStayState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => duation <= timer, Consts.S_ChaseState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocity((_ptf.position - _mmc.transform.position).normalized * (_info as BatInfo).sprintSpeed);
        timer = 0f;
    }
    public override void OnUpdate()
    {
        timer += Time.deltaTime;
        if(_hh.DetectAttackHit(raycastHits, _info.targetMask, out int hitCount))
        {
            //foreach (var c in targetCollider)
            //{
            //    if(c != null && c.CompareTag("Player") && !detectedSet.Contains(c))
            //    {
            //        EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 1.5f : -1.5f));
            //        detectedSet.Add(c);
            //        MonoMgr.Instance.StartCoroutine(DelayChageState(Consts.S_ChaseState, 0.3f));
                    
            //    }
            //    else
            //    {
            //        MonoMgr.Instance.StartCoroutine(DelayChageState(Consts.S_ChaseState, 0.3f));
            //    }
            //}
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitObject = raycastHits[i].transform.gameObject;
                if (!detectedSet.Contains(hitObject) && hitObject.CompareTag("Player"))
                {
                    EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 1.5f : -1.5f));
                    detectedSet.Add(hitObject);
                    MonoMgr.Instance.StartCoroutine(DelayChageState(Consts.S_ChaseState, 0.3f));
                }
                else
                {
                    MonoMgr.Instance.StartCoroutine(DelayChageState(Consts.S_ChaseState, 0.3f));
                }
            }

        }
    }

    public IEnumerator DelayChageState(string stateName ,float time)
    {
        yield return new WaitForSeconds(time);
        _fsm.ChangeState(stateName);
    }
}
