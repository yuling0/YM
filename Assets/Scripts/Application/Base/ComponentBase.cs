using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ComponentBase : MonoBehaviour, IInit<Core>
{
    protected Core _core;
    public virtual void Init(Core core)
    {
        _core = core;
    }

    public int ID => _core.ID;
    public string UnitName => _core.UnitName;
    public virtual T1 GetComponentInCore<T1>() where T1 : ComponentBase
    {
        return _core.GetComponentInCore<T1>();
    }

    public virtual void OnEnableComponent()
    {

    }
    public virtual void OnUpdateComponent()
    {

    }

    public virtual void OnFixedUpdateComponent()
    {

    }

    public virtual void OnDisableComponent()
    {

    }
}
