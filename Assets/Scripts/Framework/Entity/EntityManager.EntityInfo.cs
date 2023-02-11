using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.Entity
{
    public partial class EntityManager
    {
        private class EntityInfo :IReference
        {
            private IEntity entity;
            private IEntity parentEntity;
            private EntityStatus entityStatus;
            private List<IEntity> childEntities = new List<IEntity>();

            public EntityStatus EntityStatus { get => entityStatus; set => entityStatus = value; }
            public IEntity Entity { get => entity; }
            public IEntity ParentEntity { get => parentEntity;}

            public int ChildCount => childEntities.Count;

            public IEntity[] GetChildEntities => childEntities.ToArray();
            public void AddChildEntity(IEntity entity)
            {
                if (childEntities.Contains(entity))
                {
                    throw new System.Exception("Can not add entity which is already exist.");
                }
                childEntities.Add(entity);
            }

            public void RemoveChildEntity(IEntity entity)
            {
                if (childEntities.Contains(entity))
                {
                    childEntities.Remove(entity);
                }
            }
            public static EntityInfo Create(IEntity entity)
            {
                EntityInfo entityInfo = ReferencePool.Instance.Acquire<EntityInfo>();
                entityInfo.entity = entity;
                entityInfo.entityStatus = EntityStatus.WillInit;
                return entityInfo;
            }

            public void Clear()
            {
                entity = null;
                parentEntity = null;
                childEntities.Clear();
                entityStatus = EntityStatus.Unknown;
            }
        }
    }
    
}

