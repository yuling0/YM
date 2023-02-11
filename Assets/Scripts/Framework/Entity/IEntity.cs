using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.Entity
{
    public interface IEntity
    {
        int Id { get; }
        string AssetName { get; }
        IEntityGroup EntityGroup { get; }

        object TargetInstance { get; }

        void OnInit(int id,string assetName, IEntityGroup entityGroup, object userData);
        void OnShow(object userData);
        void OnHide(object userData);
        void OnRecycle();
        void OnAttached(IEntity childEntity, object userData);
        void OnDetached(IEntity childEntity, object userData);

        void OnAttachTo(IEntity parentEntity, object userData);
        void OnDetachFrom(IEntity parentEntity, object userData);
        void OnUpdate();
    }

}
