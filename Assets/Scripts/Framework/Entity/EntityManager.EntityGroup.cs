using System.Collections;
using System.Collections.Generic;
using YMFramework.ObjectPool;
namespace YMFramework.Entity
{
    public partial class EntityManager
    {

        private class EntityGroup : IEntityGroup
        {
            private IObjectPool<EntityObject> objectPool;
            private LinkedList<IEntity> entities;
            private string groupName;
            private LinkedListNode<IEntity> cacheEntity;
            public EntityGroup(string enityGroupName,ObjectPoolManager objectPoolManager)
            {
                objectPool = objectPoolManager.CreateObjectPool<EntityObject>($"Entity Object Pool {enityGroupName}");
                entities = new LinkedList<IEntity>();
                groupName = enityGroupName;

            }
            public string GroupName 
            {
                get { return groupName; }
            }

            public int Count => entities.Count;

            public bool HasEntity(int id)
            {
                foreach (var entity in entities)
                {
                    if (entity.Id == id)
                    {
                        return true;
                    }
                }
                return false;
            }

            public bool HasEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new System.Exception("Entity asset name is invalid.");
                }
                foreach (var entity in entities)
                {
                    if (entity.AssetName == entityAssetName)
                    {
                        return true;
                    }
                }
                return false;
            }

            public IEntity GetEntity(int id)
            {
                foreach (var entity in entities)
                {
                    if (entity.Id == id)
                    {
                        return entity;
                    }
                }
                return null;
            }

            public IEntity GetEntity(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new System.Exception("Entity asset name is invalid.");
                }
                foreach (var entity in entities)
                {
                    if (entity.AssetName == entityAssetName)
                    {
                        return entity;
                    }
                }
                return null;
            }

            public IEntity[] GetEntities(string entityAssetName)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new System.Exception("Entity asset name is invalid.");
                }
                List<IEntity> results = new List<IEntity>();
                foreach (var entity in entities)
                {
                    if (entity.AssetName == entityAssetName)
                    {
                        results.Add(entity);
                    }
                }
                return results.ToArray();
            }

            public void GetEntities(string entityAssetName, List<IEntity> results)
            {
                if (string.IsNullOrEmpty(entityAssetName))
                {
                    throw new System.Exception("Entity asset name is invalid.");
                }
                if (results == null)
                {
                    throw new System.Exception("Results is invalid");
                }
                results.Clear();
                foreach (var entity in entities)
                {
                    if (entity.AssetName == entityAssetName)
                    {
                        results.Add(entity);
                    }
                }
            }

            public void AddEntity(IEntity entity)
            {
                if (entity == null)
                {
                    throw new System.Exception("Entity is invalid.");
                }
                entities.AddLast(entity);
            }

            public void RemoveEntity(IEntity entity)
            {
                if(!entities.Remove(entity))
                {
                    throw new System.Exception($"Entity group {groupName} not exist specified entity {entity.Id} : {entity.AssetName}");
                }
            }

            public void RigisterEntityObject(EntityObject entityObject,bool spawned)
            {
                objectPool.Register(entityObject, spawned);
            }

            public EntityObject SpawnEntityObject(string name)
            {
                return objectPool.Spawn(name);
            }

            public void UnspawnEntityObject(IEntity entity)
            {
                objectPool.UnSpawn(entity);
            }
            public void OnUpdate()
            {
                cacheEntity = entities.First;
                while(cacheEntity != null)
                {
                    cacheEntity.Value.OnUpdate();
                    cacheEntity = cacheEntity.Next;
                }
            }
        }
    }
}
