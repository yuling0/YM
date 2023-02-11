using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YMFramework.BehaviorTreeEditor
{
    public partial class XN_GlobalClock : SingletonBase<XN_GlobalClock>
    {
        XN_Clock clock;
        XN_Blackboard blackboard;
        private XN_GlobalClock()
        {
            MonoMgr.Instance.AddUpateAction(OnUpdate);
            clock = new XN_Clock();
            
        }

        public XN_Blackboard GetBlackboard()
        {
            if(blackboard == null)
            {
                blackboard = new XN_Blackboard(this);
            }
            return blackboard;
        }

        public void AddTimer(float delay, int repeat, UnityAction action)
        {
            clock.AddTimer(delay,repeat,action);
        }

        public void RemoveTimer(UnityAction action)
        {
            clock.RemoveTimer(action);
        }

        public void AddObserver(UnityAction action)
        {
            clock.AddObserver(action);
        }

        public void RemoveObserver(UnityAction action)
        {
            clock.RemoveObserver(action);
        }
        public void OnUpdate()
        {
            clock.OnUpdate(Time.deltaTime);
        }


    }
}

