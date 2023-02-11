using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMonsterUIInfoArgs : EventArgs
{
    public MonsterAttribute _monsterAbility;
    /// <summary>
    /// 要到达的目标血量值
    /// </summary>
    public int _targetValue;
    public static UpdateMonsterUIInfoArgs Create(MonsterAttribute monsterAbility , int targetValue)
    {
        var args = ReferencePool.Instance.Acquire<UpdateMonsterUIInfoArgs>();
        args._monsterAbility = monsterAbility; ;
        args._targetValue = targetValue;
        return args;
    }

    public override void Clear()
    {
        _monsterAbility = null;
        _targetValue = 0;
    }
}
