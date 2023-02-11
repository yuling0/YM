using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextEventArgs : EventArgs
{
    private int damage;
    private Vector2 screenPosition;

    public int Damage => damage;
    public Vector2 ScreenPosition => screenPosition;

    public static DamageTextEventArgs Create(int damage, Vector2 screenPosition)
    {
        DamageTextEventArgs args = ReferencePool.Instance.Acquire<DamageTextEventArgs>();
        args.damage = damage;
        args.screenPosition = screenPosition;
        return args;
    }
    public override void Clear()
    {
        damage = 0;
        screenPosition = Vector2.zero;
    }
}
