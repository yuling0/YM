using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKickStayState : PlayerBaseState
{
    HitHandler hh;
    private RaycastHit2D[] raycastHits;

    public JumpKickStayState(BaseFSM fsm, Core core, string normalAnim, string battleAnim = null) : base(fsm, core, normalAnim, battleAnim)
    {
        AddTargetState(() => _mc.IsGrounded && _ac.CurAnimNormalizedTime >= 1, Consts.S_JumpKickNoHit);
        hh = _core.GetComponentInCore<HitHandler>();

        raycastHits = new RaycastHit2D[1];
    }
    public override void OnEnter()
    {
        base.OnEnter();
        //_mc.SetVelocity(_mc.IsFacingRight ? _erqieInfo.JumpKickVelocity.x : -_erqieInfo.JumpKickVelocity.x ,
        //    _erqieInfo.JumpKickVelocity.y);
        _mc.SetVelocityY(_erqieInfo.JumpKickVelocity.y);
    }

    public override void OnUpdate()
    {
        if (hh.DetectAttackHit(raycastHits, _erqieInfo.targetMask,out int hitCount))
        {
            //Debug.Log(c2.gameObject.name);
            _fsm.ChangeState(Consts.S_JumpKickHit);
            return;
        }
    }
    public override void OnFixedUpdate()
    {
        _mc.OptimizeFall();
        _mc.SetVelocityX(_mc.IsFacingRight ? _erqieInfo.JumpKickVelocity.x : -_erqieInfo.JumpKickVelocity.x, false);
        _fsm.GenerateGhosting();
    }
}
