using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using static YMFramework.BehaviorTree.Action;

public class PterosaurAI : BehaviorTreeController
{
    PterosaurMovementController pmc;
    PterosaurInfo info;
    public float switchBehaviourTimeInCloseRange;
    public float switchBehaviourTimeInRemoteRange;
    public float aiBehaviourTimer;
    public float jetFlameEndTime;
    public float nextEmitTime;
    public ParticleSystem flameThrower;
    protected override void InitTree()
    {
        pmc = GetComponentInCore<PterosaurMovementController>();
        info = _core.info as PterosaurInfo;
    }

    protected override void InitFunctionMap()
    {
        AddDelegate("ResetAITimer", (System.Action)ResetAITimer);
        AddDelegate("SetAI", (System.Action)SetAI);
        AddDelegate("ResetAIBehaviour", (System.Action)ResetAIBehaviour);
        AddDelegate("SetVelocityZore", (System.Action)pmc.SetVelocityZore);
        AddDelegate("IsNotFacingTarget", (Func<bool>)IsNotFacingTarget);
        AddDelegate("IsInCloseCombatRange", (Func<bool>)IsInCloseCombatRange);
        AddDelegate("Flip", (System.Action)Flip);

        AddDelegate("CheckSmashIsNeedFlip", (Func<bool>)CheckSmashIsNeedFlip);
        AddDelegate("CheckDiveIsNeedFlip", (Func<bool>)CheckDiveIsNeedFlip);
        AddDelegate("CheckHeadButtIsNeedFlip", (Func<bool>)CheckHeadButtIsNeedFlip);
        AddDelegate("CheckTailFlickIsNeedFlip", (Func<bool>)CheckTailFlickIsNeedFlip);

        AddDelegate("CheckIsInSmashRange", (Func<bool>)CheckIsInSmashRange);
        AddDelegate("CheckIsInDiveRange", (Func<bool>)CheckIsInDiveRange);
        AddDelegate("CheckIsInHeadButtRange", (Func<bool>)CheckIsInHeadButtRange);
        AddDelegate("CheckIsInTailFlickRange", (Func<bool>)CheckIsInTailFlickRange);
        AddDelegate("CheckIsInJetFlameRange", (Func<bool>)CheckIsInJetFlameRange);

        AddDelegate("SmashChaseMovement", (System.Action)SmashChaseMovement);
        AddDelegate("DiveChaseMovement", (System.Action)DiveChaseMovement);
        AddDelegate("HeadButtChaseMovement", (System.Action)HeadButtChaseMovement);
        AddDelegate("TailFlickChaseMovement", (System.Action)TailFlickChaseMovement);
        AddDelegate("SmashChaseMovementInRemoteRange", (System.Action)SmashChaseMovementInRemoteRange);

        AddDelegate("SmashSetVelocityAction", (System.Action)SmashSetVelocityAction);
        AddDelegate("DiveAnitcipationSetVelocityAction", (System.Action)DiveAnitcipationSetVelocityAction);
        AddDelegate("DiveSetVelocityAction", (System.Action)DiveSetVelocityAction);
        AddDelegate("SmashCheckGroundAction", (Func<bool, E_Result>)SmashCheckGroundAction);
        AddDelegate("JetFlameStart", (System.Action)JetFlameStart);
        AddDelegate("JetFlameAction", (Func<bool, E_Result>)JetFlameAction);
    }

    private bool IsNotFacingTarget() => !pmc.IsFacingTarget();

    private void ResetAITimer()
    {
        print("重置AI");
        aiBehaviourTimer = 0f;
        blackboard.SetBool("ResetAI", false);
    }
    public void SetHert()
    {
        blackboard.SetTrigger("IsHert");
    }
    private void SetAI()
    {
        print("重置AI");
        blackboard.SetBool("ResetAI", true);
    }
    private void Flip()
    {
        pmc.Flip();
        pmc.SetVelocityZore();
        Debug.Log("Flip调用");
    }
    private void ResetAIBehaviour()
    {
        //blackboard.SetTrigger("ResetAI");
        blackboard.SetBool("ResetAI", true);
    }
    private bool IsInCloseCombatRange()
    {
        return pmc.IsInCloseCombatRange();
    }

    private void SetAITrigger(bool isCloseRangeSkill)
    {
        aiBehaviourTimer += Time.deltaTime;
        if (aiBehaviourTimer > (isCloseRangeSkill ? switchBehaviourTimeInCloseRange : switchBehaviourTimeInRemoteRange))
        {
            ResetAIBehaviour();
        }

        if (isCloseRangeSkill && !IsInCloseCombatRange() || !isCloseRangeSkill && IsInCloseCombatRange())
        {
            ResetAIBehaviour();
        }
    }

    //这里应该实现带参数的Condition节点，而不是现在手动传参数
    #region 追击移动
    private void DiveChaseMovement()
    {
        pmc.HorizontalFollow(info.diveXAxisDistanceRange);
        pmc.VerticalFollow(info.diveYAxisDistanceRange);
        SetAITrigger(false);
    }

    private void JetFlameChaseMovement()
    {
        pmc.HorizontalFollow(info.jetFlameXAxisDistanceRange);
        pmc.VerticalFollow(info.jetFlameYAxisDistanceRange);
        SetAITrigger(false);
    }

    private void SmashChaseMovement()
    {
        pmc.HorizontalFollow(info.smashXAxisDistanceRange);
        SetAITrigger(true);
    }

    private void SmashChaseMovementInRemoteRange()
    {
        pmc.HorizontalFollow(info.smashXAxisDistanceRange);
        pmc.VerticalFollow(info.smashYAxisDistanceRange);
        SetAITrigger(false);
    }

    private void HeadButtChaseMovement()
    {
        pmc.HorizontalFollow(info.headButtXAxisDistanceRange);
        pmc.VerticalFollow(info.headButtYAxisDistanceRange);
        SetAITrigger(true);
    }

    private void TailFlickChaseMovement()
    {
        pmc.HorizontalFollow(info.tailFlickXAxisDistanceRange);
        pmc.VerticalFollow(info.tailFlickYAxisDistanceRange);
        SetAITrigger(true);
    }
    #endregion

    #region 检查追击时是否需要转向
    private bool CheckDiveIsNeedFlip()
    {
        return pmc.IsNeedFlip(info.diveXAxisDistanceRange);
    }

    private bool CheckJetFlameIsNeedFlip()
    {
        return pmc.IsNeedFlip(info.jetFlameXAxisDistanceRange);
    }
    private bool CheckSmashIsNeedFlip()
    {
        return pmc.IsNeedFlip(info.smashXAxisDistanceRange);
    }

    private bool CheckHeadButtIsNeedFlip()
    {
        return pmc.IsNeedFlip(info.headButtXAxisDistanceRange);
    }
    private bool CheckTailFlickIsNeedFlip()
    {
        return pmc.IsNeedFlip(info.tailFlickXAxisDistanceRange);
    }
    #endregion

    #region 检查是否在该技能的攻击范围
    private bool CheckIsInDiveRange()
    {
        return pmc.IsXAxisInRange(info.diveXAxisDistanceRange) && pmc.IsYAxisInRange(info.diveYAxisDistanceRange);
    }

    private bool CheckIsInJetFlameRange()
    {
        return pmc.IsXAxisInRange(info.jetFlameXAxisDistanceRange) && pmc.IsYAxisInRange(info.jetFlameYAxisDistanceRange);
    }
    private bool CheckIsInSmashRange()
    {
        return pmc.IsXAxisInRange(info.smashXAxisDistanceRange);
    }

    private bool CheckIsInHeadButtRange()
    {
        return pmc.IsXAxisInRange(info.headButtXAxisDistanceRange) && pmc.IsYAxisInRange(info.headButtYAxisDistanceRange);
    }
    private bool CheckIsInTailFlickRange()
    {
        return pmc.IsXAxisInRange(info.tailFlickXAxisDistanceRange) && pmc.IsYAxisInRange(info.tailFlickYAxisDistanceRange);
    }
    #endregion

    #region 实际的技能行动
    private void SmashSetVelocityAction()
    {
        pmc.SetVelocity(pmc.IsFacingRight ? info.smashSpeed.x : -info.smashSpeed.x, info.smashSpeed.y);
        pmc.SetNextCheckGroundTime();
    }
    private E_Result SmashCheckGroundAction(bool stop)
    {
        if (stop)
        {
            return E_Result.FAILED;
        }
        if (pmc.IsGrounded)
        {
            return E_Result.SUCESS;
        }
        pmc.SimulatedGravity();
        return E_Result.PROCESSING;
    }

    private void DiveAnitcipationSetVelocityAction()
    {
        pmc.SetVelocity(pmc.IsFacingRight ? info.diveAnitcipationSpeed.x : -info.diveAnitcipationSpeed.x, info.diveAnitcipationSpeed.y);
    }

    private void DiveSetVelocityAction()
    {
        pmc.SetVelocity(pmc.IsFacingRight ? info.diveSpeed.x : -info.diveSpeed.x, -info.diveSpeed.y);
    }

    private void JetFlameStart()
    {
        jetFlameEndTime = Time.time + info.jetFlameDurationTime;
        nextEmitTime = Time.time;
    }
    private E_Result JetFlameAction(bool stop)
    {
        if (stop)
        {
            return E_Result.FAILED;
        }
        if (Time.time >= jetFlameEndTime)
        {
            return E_Result.SUCESS;
        }
        if (Time.time >= nextEmitTime)
        {
            flameThrower.Emit(UnityEngine.Random.Range(info.emitCountRange.x, info.emitCountRange.y));
            nextEmitTime = Time.time + info.generateFlameParticleIntervalTime;
        }
        return E_Result.PROCESSING;
    }
    #endregion
}
