using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttibute : ComponentBase
{
    //组件
    protected CombatInfo _info;
    protected BaseFSM _fsm;
    protected MovementController _mc;
    private SpriteRenderer _spriteRenderer;
    //属性
    protected int _maxHP;
    protected int _curHP;
    protected int _def;
    protected int _atk;

    public int MaxHP { get => _maxHP;  }
    public int CurHP { get => _curHP;  }
    public SpriteRenderer SpriteRenderer { get => _spriteRenderer; }

    public sealed override void Init(Core core)
    {
        base.Init(core);
        _info = core.info as CombatInfo;
        _fsm = core.GetComponentInCore<BaseFSM>();
        _mc = core.GetComponentInCore<MovementController>();
        _spriteRenderer = _core.GetComponentInCore<AnimationController>().SpriteRenderer;
        InitAbility();
    }

    protected virtual void InitAbility()
    {
        _maxHP = _info.hp;
        _curHP = _maxHP;
        _def = _info.def;
        _atk = _info.atk;
    }
}
