using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBaseState : BaseState
{
    protected MonsterMovementController _mmc;
    protected MonsterFSM _mfsm;
    protected Transform _monsterTF;
    protected Transform _ptf;
    protected MonsterInfo _info;
    protected AnimationController _ac;
    protected string _animName;
    protected Vector2 _distance;         //与玩家的距离向量
    public MonsterBaseState(BaseFSM fsm, Core core , string animName) : base(fsm, core)
    {
        _mfsm = fsm as MonsterFSM;
        _mmc = core.GetComponentInCore<MonsterMovementController>();
        _ac = core.GetComponentInCore<AnimationController>();
        _ptf = GameManager.Instance.PlayerTF;
        _info = core.info as MonsterInfo;
        _animName = animName;
        _monsterTF = _mmc.transform;
    }
    public override void OnEnter()
    {
        base.OnEnter();
        _ac.PlayAnim(_animName);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        _distance = _ptf.position - _mmc.transform.position;
    }
}
