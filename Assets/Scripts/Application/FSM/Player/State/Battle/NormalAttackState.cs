using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAttackState : PlayerAttackState
{
    bool canCombo;
    public NormalAttackState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => canCombo && ( _sh.GetCurrentSkillName() == Consts.J_WideSlashLeft 
        || _sh.GetCurrentSkillName() == Consts.J_WideSlashRight ), Consts.S_ComboAttackForward);

        AddTargetState(() => canCombo && _sh.GetCurrentSkillName() == Consts.J_UpSlash , Consts.S_ComboAttackUp);

        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_Idle);

    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.SetFriction();
        canCombo = false;
    }

    public override void AnimationEventTrigger()
    {
        canCombo = true;
    }
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        _mc.AddFriction(0.15f);
    }
    public override void OnExit()
    {
        base.OnExit();
        //_mc.SetSmooth();
    }
}
