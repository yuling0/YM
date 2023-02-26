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
    private float _knockbackValue;
    private float _knockupVlaue;
    private Vector2 hitPoint;
    /// <summary>
    /// 被击中的游戏对象
    /// </summary>
    public GameObject MonsterObj => _monsterObj;

    /// <summary>
    /// 伤害量
    /// </summary>
    public int DamageValue => _damageVal;

    /// <summary>
    /// 击退值
    /// </summary>
    public float KnockbackValue => _knockbackValue;

    /// <summary>
    /// 击飞值
    /// </summary>
    public float KnockupValue => _knockupVlaue;

    /// <summary>
    /// 击中点的屏幕坐标
    /// </summary>
    public Vector2 HitPoint => hitPoint;
    public static MonsterBeHitEventArgs Create(Vector2 hitPoint ,GameObject go, int damageVal, float knockbackValue = 0f , float knockupVlaue = 0f)
    {
        var args = ReferencePool.Instance.Acquire<MonsterBeHitEventArgs>();
        args._monsterObj = go;
        args._damageVal = damageVal;
        args._knockbackValue = knockbackValue;
        args._knockupVlaue = knockupVlaue;
        args.hitPoint = hitPoint;
        return args;
    }

    public override void Clear()
    {
        _monsterObj = null;
        _damageVal = 0;
        _knockbackValue = 0f;
        _knockupVlaue = 0f;
        hitPoint = default(Vector2);
    }
}
