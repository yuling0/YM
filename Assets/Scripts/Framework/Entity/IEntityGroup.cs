using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.Entity
{
    public interface IEntityGroup
    {
        string GroupName { get; }

        int Count { get; }

        bool HasEntity(int id);

        bool HasEntity(string entityName);

        IEntity GetEntity(int id);

        IEntity GetEntity(string entityName);

        IEntity[] GetEntities(string entityName);

        void GetEntities(string entityName, List<IEntity> results);

        void OnUpdate();
    }

}
