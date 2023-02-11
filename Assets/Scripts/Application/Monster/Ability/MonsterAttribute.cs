using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttribute : CharacterAttibute
{
    protected MonsterFSM _monsterFSM;
    protected MonsterInfo _monsterInfo;
    protected MonsterMovementController _monsterMovementController;
    protected MonsterAI monsterAI;
    private bool isHert;
    public float effectOffset;          //特效y轴偏移（后面应该要改）
    protected override void InitAbility()
    {
        base.InitAbility();
        _monsterFSM = _fsm as MonsterFSM;
        _monsterInfo = _info as MonsterInfo;
        _monsterMovementController = _mc as MonsterMovementController;
        monsterAI = GetComponentInCore<MonsterAI>();
        //TODO:监听受伤事件，记得怪物死亡时移除
        EventMgr.Instance.AddMultiParameterEventListener<MonsterBeHitEventArgs>(Hert);
    }

    public bool IsHert
    {
        get => isHert;
        set => isHert = value;
    }

    public bool GetHertInfo() => isHert;
    protected void Hert(MonsterBeHitEventArgs args)
    {
        if(args.MonsterObj == this.gameObject)
        {
            //_monsterFSM.ChangeState(Consts.S_HurtState);

            _monsterMovementController.SetVelocityX(args.RepelVelocity,false);

            PoolMgr.Instance.PopObj<GameObject>(Consts.P_Hit).transform.position = transform.position + Vector3.up * effectOffset;

            int targetHp = CurHP >= args.DamageValue ? CurHP - args.DamageValue : 0;

            monsterAI.SetHert();

            isHert = true;

            EventMgr.Instance.OnMultiParameterEventTrigger<UpdateMonsterUIInfoArgs>(UpdateMonsterUIInfoArgs.Create(this, targetHp));

            _curHP = targetHp;

        }
    }

    protected void Death()
    {

    }
}
