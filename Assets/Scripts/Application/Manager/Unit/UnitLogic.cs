using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLogic : MonoBehaviour
{
    protected Unit unit;

    public virtual void OnInit(Unit unit,object userData)
    {
        this.unit = unit;
    }

    public virtual void OnShow(object userData) { }

    public virtual void OnHide(object userData) { }

    public virtual void OnRecycle(object userData) { }

    public virtual void OnUpdate() { }

    public virtual void OnFixedUpdate() { }

}
