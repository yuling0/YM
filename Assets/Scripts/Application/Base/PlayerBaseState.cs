using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBaseState : BaseState
{
    protected AnimationController _ac;
    protected PlayerMovementController _mc;
    protected InputHandler _ih;
    protected SkillHandler _sh;
    protected string normalAnimName;
    protected string battleAnimName;
    protected PlayerFSM _pfsm;
    protected ArcheInfo _erqieInfo;
    public PlayerBaseState(BaseFSM fsm, Core core ,string normalAnim ,string battleAnim = null):base(fsm,core)
    {
        _ac = core.GetComponentInCore<AnimationController>();
        _mc = core.GetComponentInCore<PlayerMovementController>();
        _ih = core.GetComponentInCore<InputHandler>();
        _sh = core.GetComponentInCore<SkillHandler>();
        normalAnimName = normalAnim;
        battleAnimName = battleAnim;
        _pfsm = fsm as PlayerFSM;
        _erqieInfo = _core.info as ArcheInfo;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _ac.PlayAnim(_pfsm.isBattleState ? battleAnimName : normalAnimName);
    }
}
