using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 怪物受击事件参数
/// </summary>
public class MonsterBeHitEventArgs : EventArgs
{
    private GameObject _monsterObj;
    private int _damageVal;
    private float _repelVelocity;

    /// <summary>
    /// 被击中的游戏对象
    /// </summary>
    public GameObject MonsterObj => _monsterObj;

    /// <summary>
    /// 伤害量
    /// </summary>
    public int DamageValue => _damageVal;

    /// <summary>
    /// 击退速度
    /// </summary>
    public float RepelVelocity => _repelVelocity;
    public static MonsterBeHitEventArgs Create(GameObject go, int harmVal, float repelVelocity = 0f)
    {
        var args = ReferencePool.Instance.Acquire<MonsterBeHitEventArgs>();
        args._monsterObj = go;
        args._damageVal = harmVal;
        args._repelVelocity = repelVelocity;
        return args;
    }

    public override void Clear()
    {
        _monsterObj = null;
        _damageVal = 0;
        _repelVelocity = 0f;
    }
}
