using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.Entity;

public class Entity : MonoBehaviour, IEntity
{
    private int id;
    private string assetName;
    private IEntityGroup entityGroup;
    public int Id => id;

    public string AssetName => assetName;

    public IEntityGroup EntityGroup => entityGroup;

    public object TargetInstance => gameObject;

    public virtual void OnAttached(IEntity childEntity, object userData)
    {
        
    }

    public virtual void OnAttachTo(IEntity parentEntity, object userData)
    {
        
    }

    public virtual void OnDetached(IEntity childEntity, object userData)
    {
        
    }

    public virtual void OnDetachFrom(IEntity parentEntity, object userData)
    {
        
    }

    public virtual void OnHide(object userData)
    {
        
    }

    public virtual void OnInit(int id, string assetName, IEntityGroup entityGroup, object userData)
    {
        this.id = id;
        this.assetName = assetName;
        this.entityGroup = entityGroup;
    }

    public virtual void OnRecycle()
    {
        
    }

    public virtual void OnShow(object userData)
    {

    }
    public virtual void OnUpdate()
    {

    }
}
