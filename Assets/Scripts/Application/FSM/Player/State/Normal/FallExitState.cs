using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallExitState : PlayerBaseState
{
    public FallExitState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        //AddTargetState(() => _mc.IsGrounded && _mc.IsFacingRight && _ih.isRightRunning || !_mc.IsFacingRight && _ih.isLeftRunning, Consts.S_Run);

        AddTargetState(() =>
        {
            _mc.SetVelocityX(_pfsm.skillBufferSpeed != 0 ? _pfsm.skillBufferSpeed : _pfsm.bufferSpeed);
            return _mc.IsGrounded && !_mc.IsFacingRight && _ih.isLeftPress || _mc.IsFacingRight && _ih.isRightPress;
        }, Consts.S_Run);

        AddTargetState(() => 
        {
            _mc.SetVelocityX(_pfsm.skillBufferSpeed != 0 ? _pfsm.skillBufferSpeed : _pfsm.bufferSpeed);
            return _pfsm.skillBufferSpeed != 0 || _pfsm.bufferSpeed != 0; 
            
        }, Consts.S_Buffer);
        AddTargetState(() => _ih.jumpPress, Consts.S_Jump);
        AddTargetState(() => _mc.IsGrounded && _ac.CurAnimNormalizedTime >= 1f, Consts.S_Idle);
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.SetFriction();
        _mc.SetVelocityZore();
        //if (_ih.AbsH == 0 && (_pfsm.skillBufferSpeed != 0 || _pfsm.bufferSpeed != 0))
        //{
        //    _mc.SetVelocityX(_pfsm.skillBufferSpeed != 0 ? _pfsm.skillBufferSpeed : _pfsm.bufferSpeed);

        //    _pfsm.ChangeState(Consts.S_Buffer);

        //}
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
    }
    public override void OnExit()
    {
        base.OnExit();
        _pfsm.skillBufferSpeed = 0;
        _pfsm.bufferSpeed = 0;

        Debug.Log($"落地后的速度：{_mc.Velocity.x}");
    }
}
