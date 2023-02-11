using System;
using System.Collections.Generic;
using UnityEngine.Events;
/// <summary>
/// 事件中心模块
/// </summary>
public class EventMgr : SingletonBase<EventMgr>
{
    private Dictionary<Type, IEventSubject> typeEventDic;          //Type类型的事件容器
    private Dictionary<string, IEventSubject> stringEventDic;            //string类型的事件容器
    private EventMgr()
    {
        typeEventDic = new Dictionary<Type, IEventSubject>();
        stringEventDic = new Dictionary<string, IEventSubject>();
    }

    #region Type类型的事件
    public void AddMultiParameterEventListener<T>(UnityAction<T> action) where T : IEventArgs
    {

        if(!typeEventDic.TryGetValue(typeof(T),out IEventSubject targetEvent))
        {
            targetEvent = new EventSubject<T>();
            typeEventDic.Add(typeof(T), targetEvent);
        }

        (targetEvent as EventSubject<T>).AddEventListener(action);
    }

    public void RemoveMultiParameterEventListener<T>(UnityAction<T> action) where T : IEventArgs
    {
        if (typeEventDic.TryGetValue(typeof(T), out IEventSubject targetEvent))
        {
            (targetEvent as EventSubject<T>).RemoveEventListener(action);
        }
    }

    public void OnMultiParameterEventTrigger<T>(T args) where T : IEventArgs
    {
        if (typeEventDic.TryGetValue(typeof(T), out IEventSubject targetEvent))
        {
            (targetEvent as EventSubject<T>).OnTrigger(args);
        }
    }

    #endregion

    #region string类型的事件
    public void AddEventListener<T>(string eventName, UnityAction<T> action)
    {

        if (!stringEventDic.TryGetValue(eventName, out IEventSubject targetEvent))
        {
            targetEvent = new EventSubject<T>();
            stringEventDic.Add(eventName, targetEvent);
        }

        (targetEvent as EventSubject<T>).AddEventListener(action);
    }

    public void AddEventListener(string eventName, UnityAction action)
    {

        if (!stringEventDic.TryGetValue(eventName, out IEventSubject targetEvent))
        {
            targetEvent = new EventSubject();
            stringEventDic.Add(eventName, targetEvent);
        }

        (targetEvent as EventSubject).AddEventListener(action);
    }
    public void AddEventListener<TResult>(string eventName, Func<TResult> action)
    {
        if (!stringEventDic.TryGetValue(eventName, out IEventSubject targetEvent))
        {
            targetEvent = new FuncEventSubject<TResult>();
            stringEventDic.Add(eventName, targetEvent);
        }
    (targetEvent as FuncEventSubject<TResult>).AddEventListener(action);
    }
    public void AddEventListener<T,TResult>(string eventName, Func<T,TResult> action)
    {
        if (!stringEventDic.TryGetValue(eventName, out IEventSubject targetEvent))
        {
            targetEvent = new FuncEventSubject<T, TResult>();
            stringEventDic.Add(eventName, targetEvent);
        }
        (targetEvent as FuncEventSubject<T,TResult>).AddEventListener(action);
    }

    public void RemoveEventListener<T>(string eventName , UnityAction<T> action)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as EventSubject<T>).RemoveEventListener(action);
        }
    }
    public void RemoveEventListener(string eventName , UnityAction action)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as EventSubject).RemoveEventListener(action);
        }
    }
    public void RemoveEventListener<TResult>(string eventName, Func<TResult> action)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as FuncEventSubject<TResult>).RemoveEventListener(action);
        }
    }
    public void RemoveEventListener<T, TResult>(string eventName, Func<T, TResult> action)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as FuncEventSubject<T, TResult>).RemoveEventListener(action);
        }
    }

    public void OnEventTrigger<T>(string eventName, T data)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as EventSubject<T>).OnTrigger(data);
        }
    }

    public void OnEventTrigger(string eventName)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            (stringEventDic[eventName] as EventSubject).OnTrigger();
        }
    }
    public TResult OnEventTrigger<TResult>(string eventName)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            return (stringEventDic[eventName] as FuncEventSubject<TResult>).OnTrigger();
        }
        throw new Exception($"{eventName} is invalid");
    }
    public TResult OnEventTrigger<T,TResult>(string eventName,T data)
    {
        if (stringEventDic.ContainsKey(eventName))
        {
            return (stringEventDic[eventName] as FuncEventSubject<T, TResult>).OnTrigger(data);
        }
        throw new Exception($"{eventName} is invalid");
    }
    #endregion
}
