using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : UnitLogic
{
    public ParticleSystem ps;
    public float duation;

    public override void OnShow(object userData)
    {
        base.OnShow(userData);
        ps?.Play();
        Invoke("PushPool", duation);
    }
    private void PushPool()
    {
        UnitManager.Instance.HideUnit(this.unit,null);
    }
}
