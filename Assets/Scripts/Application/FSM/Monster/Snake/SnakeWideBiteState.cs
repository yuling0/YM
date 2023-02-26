using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeWideBiteState : MonsterAttackState
{
    SnakeInfo snakeInfo;
    public SnakeWideBiteState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        snakeInfo = (_info as SnakeInfo);
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_ChaseState);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        _mmc.SetVelocity(_mmc.IsFacingRight ? snakeInfo.sprintSpeed.x : -snakeInfo.sprintSpeed.x, snakeInfo.sprintSpeed.y);
    }
    protected override void DetectContinuousAttack()
    {
        if (_hh.DetectAttackHit(raycastHits, _info.targetMask , out int hitCount))
        {
            //foreach (var collider in targetCollider)
            //{
            //    if (collider.CompareTag("Player") && !detectedSet.Contains(collider))
            //    {
            //        EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
            //        detectedSet.Add(collider);
            //        _mmc.SetVelocityX(-0.1f);
            //        _fsm.ChangeState(Consts.S_WideBiteHitState);
            //        break;
            //    }
            //}
            for (int i = 0; i < hitCount; i++)
            {
                GameObject hitGameObject = raycastHits[i].transform.gameObject;
                if (hitGameObject.CompareTag("Player"))
                {
                    EventMgr.Instance.OnMultiParameterEventTrigger(PlayerBeHitEventArgs.Create(0, _mmc.IsFacingRight ? 2 : -2));
                    detectedSet.Add(hitGameObject);
                    _mmc.SetVelocityX(-0.1f);
                    _fsm.ChangeState(Consts.S_WideBiteHitState);
                    break;
                }
            }
        }
    }

}
