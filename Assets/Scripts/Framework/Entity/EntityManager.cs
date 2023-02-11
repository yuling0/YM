using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.ObjectPool;

namespace YMFramework.Entity
{
    public partial class EntityManager
    {
        private Dictionary<int, EntityInfo> entityInfoDic;
        private Dictionary<string, EntityGroup> groupDic;
        private HashSet<int> entitiesBeingLoaded;
        private Queue<EntityInfo> entityRecycleQueue;
        private ObjectPoolManager objectPoolManager;
        private ResourceMgr resourceMgr;
        private IEntityHelper entityHelper;

        public IEntityHelper EntityHelper 
        {
            get => entityHelper; 
            set => entityHelper = value; 
        }
        public void Update()
        {
            while(entityRecycleQueue.Count > 0)
            {
                EntityInfo entityInfo = entityRecycleQueue.Dequeue();
                EntityGroup entityGroup = entityInfo.Entity.EntityGroup as EntityGroup;
                entityInfo.EntityStatus = EntityStatus.WillRecycle;
                entityInfo.Entity.OnRecycle();
                entityInfo.EntityStatus = EntityStatus.Recycled;
                entityGroup.UnspawnEntityObject(entityInfo.Entity);
                ReferencePool.Instance.Release(entityInfo);
            }
            foreach (KeyValuePair<string,EntityGroup> entityGroup in groupDic)
            {
                entityGroup.Value.OnUpdate();
            }
        }

        public void AddEntityGroup(string groupName)
        {
            if (groupDic.ContainsKey(groupName))
            {
                throw new System.Exception($"Entity Group {groupName} is Already exist");
            }
            groupDic.Add(groupName, new EntityGroup(groupName, objectPoolManager));
        }
        public IEntityGroup GetEntityGroup(string groupName)
        {
            if (groupDic.ContainsKey(groupName))
            {
                return groupDic[groupName];
            }
            return null;
        }
        public void ShowEntity(int id,string assetName,string entityGroupName)
        {
            ShowEntity(id,assetName,entityGroupName,null);
        }

        public void ShowEntity(int id, string entityAssetName, string entityGroupName,object userData)
        {
            //尝试在对象池获取实体
            if (string.IsNullOrEmpty(entityAssetName))
            {
                throw new System.Exception("Entity asset name is invalid");
            }

            EntityGroup entityGroup = GetEntityGroup(entityGroupName) as EntityGroup;
            if (entityGroup == null)
            {
                throw new System.Exception($"Entity group {entityGroupName} is not exist");
            }

            EntityObject entityObject = entityGroup.SpawnEntityObject(entityAssetName);
            //获取不到：资源异步加载
            if (entityObject == null)
            {
                resourceMgr.LoadAssetAsync<GameObject>(entityAssetName, (obj) => 
                {
                    entityGroup.RigisterEntityObject(EntityObject.Create(entityAssetName, obj), true);
                    InternalShowEntity(id, entityAssetName, entityGroup, obj, userData);
                });
                return;
            }

            InternalShowEntity(id, entityAssetName, entityGroup, entityObject, userData);
        }

        public void HideEntity(int id)
        {
            HideEntity(id,null);
        }

        public void HideEntity(int id ,object userData)
        {
            if (entityInfoDic.ContainsKey(id))
            {
                InternalHideEntity(entityInfoDic[id], userData);
            }
        }

        private void InternalShowEntity(int id, string entityAssetName, EntityGroup entityGroup, object entityInstance, object userData)
        {
            if (entityHelper == null)
            {
                throw new System.Exception("Entity helper is invalid.");
            }
            IEntity entity = entityHelper.CreateEntity(id, entityAssetName, entityGroup, entityInstance, userData);
            EntityInfo entityInfo = EntityInfo.Create(entity);
            entityInfoDic.Add(id, entityInfo);
            entityInfo.EntityStatus = EntityStatus.WillInit;
            entity.OnInit(id, entityAssetName, entityGroup, userData);
            entityInfo.EntityStatus = EntityStatus.Inited;
            entityGroup.AddEntity(entity);
            entityInfo.EntityStatus = EntityStatus.WillShow;
            entity.OnShow(userData);
            entityInfo.EntityStatus = EntityStatus.Showed;

            //TODO:可以处理回调

        }
        private void InternalHideEntity(EntityInfo entityInfo, object userData)
        {
            entityInfo.EntityStatus = EntityStatus.WillHide;
            IEntity entity = entityInfo.Entity;
            entity.OnHide(userData);
            entityInfo.EntityStatus = EntityStatus.Hidden;
            entityRecycleQueue.Enqueue(entityInfo);

        }
    }

}
