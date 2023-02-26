using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// �����ܻ��¼�����
/// </summary>
public class MonsterBeHitEventArgs : EventArgs
{
    private GameObject _monsterObj;
    private int _damageVal;
    private float _knockbackValue;
    private float _knockupVlaue;
    private Vector2 hitPoint;
    /// <summary>
    /// �����е���Ϸ����
    /// </summary>
    public GameObject MonsterObj => _monsterObj;

    /// <summary>
    /// �˺���
    /// </summary>
    public int DamageValue => _damageVal;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public float KnockbackValue => _knockbackValue;

    /// <summary>
    /// ����ֵ
    /// </summary>
    public float KnockupValue => _knockupVlaue;

    /// <summary>
    /// ���е����Ļ����
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
