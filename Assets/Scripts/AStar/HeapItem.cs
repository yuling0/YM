using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public interface HeapItem<T> : IComparable<T>
    {
        public int HeapIndex
        {
            set;
            get;
        }
    }
}

