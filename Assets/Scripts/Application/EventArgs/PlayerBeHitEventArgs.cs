using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeHitEventArgs : EventArgs
{
    private int _harmVal;
    private float _repelVelocity;
    /// <summary>
    /// ÉËº¦Á¿
    /// </summary>
    public int HarmVal => _harmVal;

    /// <summary>
    /// »÷ÍËËÙ¶È
    /// </summary>
    public float RepelVelocity => _repelVelocity;

    public static PlayerBeHitEventArgs Create(int harmVal, float repelVelocity = 0f)
    {
        var args = ReferencePool.Instance.Acquire<PlayerBeHitEventArgs>();
        args._harmVal= harmVal;
        args._repelVelocity = repelVelocity;
        return args;
    }

    public override void Clear()
    {
        _harmVal = 0;
        _repelVelocity = 0f;
    }
}