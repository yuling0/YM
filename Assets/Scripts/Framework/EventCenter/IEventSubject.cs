using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IEventSubject
{

}

public class EventSubject : IEventSubject 
{
    UnityAction subject;

    public void AddEventListener(UnityAction action)
    {
        subject += action;
    }

    public void RemoveEventListener(UnityAction action)
    {
        subject -= action;
    }

    public void OnTrigger()
    {
        subject?.Invoke();
    }
}

public class EventSubject<T> :IEventSubject
{
    UnityAction<T> subject;

    public void AddEventListener(UnityAction<T> action)
    {
        subject += action;
    }

    public void RemoveEventListener(UnityAction<T> action)
    {
        subject -= action;
    }

    public void OnTrigger(T args)
    {
        subject?.Invoke(args);
    }
}
public class FuncEventSubject<TResult> : IEventSubject
{
    Func<TResult> subject;
    public void AddEventListener(Func<TResult> action)
    {
        subject += action;
    }

    public void RemoveEventListener(Func<TResult> action)
    {
        subject -= action;
    }

    public TResult OnTrigger()
    {
        if (subject == null)
        {
            throw new Exception("This subject is null");
        }
        return subject.Invoke();
    }
}
public class FuncEventSubject<T,TResult> : IEventSubject
{
    Func<T,TResult> subject;
    public void AddEventListener(Func<T, TResult> action)
    {
        subject += action;
    }

    public void RemoveEventListener(Func<T, TResult> action)
    {
        subject -= action;
    }

    public TResult OnTrigger(T args) 
    {
        if (subject == null)
        {
            throw new Exception("This subject is null");
        }
        return subject.Invoke(args);
    }
}