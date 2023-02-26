using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStayState : PlayerBaseState
{
    float leftPressTimer;
    float rightPressTimer;
    float bufferMultiplier = 6f;
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
            leftPressTimer += Time.deltaTime;
        }

        if (_ih.isRightWalk)
        {
            rightPressTimer += Time.deltaTime;
        }

    }

    public override void OnFixedUpdate()
    {
        _mc.Fall();
        if (_ih.AbsH == 0)
        {
            _mc.AddAirFriction(0.1f);
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        Debug.Log("����ʱ��" + Mathf.Abs(leftPressTimer - rightPressTimer));
        _pfsm.bufferSpeed = Mathf.Min(Mathf.Abs(leftPressTimer - rightPressTimer), 2f) * bufferMultiplier;
        //_mc.SetVelocityX(Mathf.Min(Mathf.Abs(leftPressTimer - rightPressTimer), 1f) * bufferSpeed);
    }

}
