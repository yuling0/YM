using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatSprintEnterState : MonsterBaseState
{
    public BatSprintEnterState(BaseFSM fsm, Core core, string animName) : base(fsm, core, animName)
    {
        AddTargetState(() => _ac.CurAnimNormalizedTime >= 1, Consts.S_SprintStayState);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mmc.SetVelocity((_ptf.position - _mmc.transform.position).normalized * (_info as BatInfo).sprintSpeed);
        (_fsm as BatFSM).attackTimer = 0f;


        if (_mfsm.transform.position.x < _ptf.transform.position.x && !_mmc.IsFacingRight || _mfsm.transform.position.x > _ptf.transform.position.x && _mmc.IsFacingRight)
        {
            _mmc.Flip();
        }
    }
}
