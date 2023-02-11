using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.Entity
{
    public interface IEntityHelper
    {
        public IEntity CreateEntity(int id, string entityAssetName, IEntityGroup entityGroup, object entityInstance, object userData);

        public void ReleaseEntity(object entityInstance);
    }
}

