using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStayState : PlayerBaseState
{
    float leftPressTimer;
    float rightPressTimer;
    float timeVariation = 8f;
    float bufferMultiplier = 4f;
    public FallStayState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _ih.isDownAttack, Consts.S_DownStabEnterState);
        AddTargetState(() => _pfsm.isBattleState && _pfsm.isCanJumpAttack &&
        _sh.CheckSkillIsCompelted(Consts.J_NormalAttack), Consts.S_NormalJumpAttack);
        AddTargetState(() => _mc.IsGrounded, Consts.S_FallExit);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        leftPressTimer = 0;
        rightPressTimer = 0;
    }

    public override void OnUpdate()
    {
        if (_ih.isLeftWalk)
        {
            leftPressTimer += Time.deltaTime * timeVariation;
        }

        if (_ih.isRightWalk)
        {
            rightPressTimer += Time.deltaTime * timeVariation;
        }

    }

    public override void OnFixedUpdate()
    {
        _mc.Fall();
    }

    public override void OnExit()
    {
        base.OnExit();
        _pfsm.bufferSpeed = Mathf.Min(Mathf.Abs(leftPressTimer - rightPressTimer), 1f) * bufferMultiplier;
        //_mc.SetVelocityX(Mathf.Min(Mathf.Abs(leftPressTimer - rightPressTimer), 1f) * bufferSpeed);
    }

}
