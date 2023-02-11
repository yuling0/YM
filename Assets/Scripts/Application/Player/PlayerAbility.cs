using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家能力类（一些属性相关）
/// </summary>
public class PlayerAbility : CharacterAttibute
{
    PlayerFSM _playerFSM;
    ArcheInfo _erQieInfo;
    PlayerMovementController _playerMovementController;
    private float _inactiveTime = 0.1f;         //无敌间隔时间
    private float _inactiveTimer = 0.1f;
    protected override void InitAbility()
    {
        base.InitAbility();
        _playerFSM = _fsm as PlayerFSM;
        _erQieInfo = _info as ArcheInfo;
        _playerMovementController = _mc as PlayerMovementController;
        EventMgr.Instance.AddMultiParameterEventListener<PlayerBeHitEventArgs>(Hurt);
    }

    private void Hurt(PlayerBeHitEventArgs args)
    {
        if (_inactiveTimer <= _inactiveTime) return;

        if(_playerFSM.GetCurrentState() is DefendState)
        {

        }
        else
        {
            _playerFSM.ChangeState(Consts.S_HurtState);
            _playerMovementController.SetVelocityX(args.RepelVelocity, false);
            _inactiveTimer = 0f;
        }


    }
    public override void OnUpdateComponent()
    {
        base.OnUpdateComponent();
        _inactiveTimer += Time.deltaTime;
    }
}
