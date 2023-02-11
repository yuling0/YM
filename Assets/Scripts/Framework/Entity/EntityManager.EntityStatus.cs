using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace YMFramework.Entity
{
    public partial class EntityManager
    {
        public enum EntityStatus : byte
        {
            Unknown = 0,
            WillInit,
            Inited,
            WillShow,
            Showed,
            WillHide,
            Hidden,
            WillRecycle,
            Recycled
        }
    }
   
}

