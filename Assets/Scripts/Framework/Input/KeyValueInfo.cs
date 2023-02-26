using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyValueInfo : KeyInfo
{
    private float keyValue;
    public float minValue;
    public float maxValue;
    public float valueOffset = 0.3f;
    public float KeyValue => keyValue;
    public KeyValueInfo(KeyCode kc) : base(kc)
    {
    }

    public override void OnUpdate()
    {
        //keyValue = Input.GetKey(_keyCode) ? Mathf.Clamp(keyValue + valueOffset, minValue, maxValue) : minValue;
    }

    public override void Disable()
    {
        
    }
}
