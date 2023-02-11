using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

namespace YMFramework.Entity
{
    public partial class EntityManager
    {

        private class EntityObject : ObjectBase
        {
            public EntityObject() { }

            public static EntityObject Create(string name , object entityInstacne ) 
            {
                EntityObject entityObject = ReferencePool.Instance.Acquire<EntityObject>();
                entityObject.Init(name, entityInstacne);
                return entityObject;
            }

            public override void Release()
            {
                Object.Destroy(Target as GameObject);
            }
        }
    }
}
