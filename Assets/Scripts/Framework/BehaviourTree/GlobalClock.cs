using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using YMFramework.BehaviorTree;

public partial class GlobalClock : SingletonBase<GlobalClock>
{
    Clock clock;
    Dictionary<string, Blackboard> sharedBlackboardDic = new Dictionary<string, Blackboard>();
    private GlobalClock()
    {
        MonoMgr.Instance.AddUpateAction(OnUpdate);
        clock = new Clock();

    }

    public Blackboard GetSharedBlackboard(string key)
    {
        if(!sharedBlackboardDic.ContainsKey(key))
        {
            sharedBlackboardDic.Add(key, new Blackboard(this));
        }
        return sharedBlackboardDic[key];
    }

    public void AddTimer(float interval, int repeat, System.Action action , System.Action onComplete = null)
    {
        clock.AddTimer(interval, repeat, action, onComplete);
    }
    public void AddTimer(float delay,float interval, int repeat, System.Action action, System.Action onComplete = null)
    {
        clock.AddTimer(delay, interval, repeat, action, onComplete);
    }

    public bool HasTimer(System.Action action)
    {
        return clock.HasTimer(action);
    }

    public void RemoveTimer(System.Action action)
    {
        clock.RemoveTimer(action);
    }

    public void AddObserver(System.Action action)
    {
        clock.AddObserver(action);
    }

    public void RemoveObserver(System.Action action)
    {
        clock.RemoveObserver(action);
    }
    public void OnUpdate()
    {
        clock.OnUpdate(Time.deltaTime);
    }


}


