using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace YMFramework.BehaviorTree
{
    public abstract class Task : Node
    {
        public Task(string name) : base(name)
        {

        }
    }
}