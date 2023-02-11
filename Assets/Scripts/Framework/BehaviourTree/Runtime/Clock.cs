using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public partial class GlobalClock
{
    private class Clock
    {

        Dictionary<System.Action, Timer> updateTimerDic = new Dictionary<System.Action, Timer>();       //计时器容器

        Dictionary<System.Action, Timer> addTimerDic = new Dictionary<System.Action, Timer>();    //添加缓冲容器
        HashSet<System.Action> removeTimerSet = new HashSet<System.Action>();     //移除缓冲容器

        List<System.Action> updateObservers = new List<System.Action>();          //需要持续更新的委托（即没有Interval时间）
        HashSet<System.Action> addObservers = new HashSet<System.Action>();       //添加缓冲
        HashSet<System.Action> removeObservers = new HashSet<System.Action>();    //移除缓冲


        bool isInUpdate = false;        //是否正在更新（即是否正在遍历容器，正在更新时，不能更改容器，只能添加至缓冲区）
        float elapsedTime = 0f;         //当前运行时间
        private class Timer : IReference
        {
            public float scheduledTime;  //预计执行的时间(下一次执行时间)
            public float delay;          //第一次执行的延迟时间
            public float interval;       //后续执行的间隔时间
            public int repeat;           //重复次数
            public System.Action OnTimerComplete;

            public static Timer CreateTimer(float delay, float interval, int repeat, System.Action OnComplete = null)
            {
                Timer t = ReferencePool.Instance.Acquire<Timer>();
                t.delay = delay;
                t.repeat = repeat;
                t.interval = interval;
                t.OnTimerComplete = OnComplete;
                return t;
            }
            public static Timer CreateTimer(float interval, int repeat, System.Action OnComplete = null)
            {
                Timer t = ReferencePool.Instance.Acquire<Timer>();
                t.delay = 0f;
                t.repeat = repeat;
                t.interval = interval;
                t.OnTimerComplete = OnComplete;
                return t;
            }
            public void Calculate(float elapsedTime, bool isUseDelay = false)
            {
                scheduledTime = elapsedTime + interval + (isUseDelay ? delay : 0f);
            }

            public void Clear()
            {
                scheduledTime = 0f;
                delay = 0f;
                interval = 0;
                repeat = 0;
                OnTimerComplete = null;
            }
        }

        /// <summary>
        /// 添加一个计时器
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="repeat"></param>
        /// <param name="action"></param>
        public void AddTimer(float interval, int repeat, System.Action action, System.Action OnComplete = null)
        {
            Timer timer = null;
            if (!isInUpdate)
            {
                timer = Timer.CreateTimer(interval, repeat, OnComplete);
                timer.Calculate(elapsedTime);
                if (!updateTimerDic.ContainsKey(action))
                {

                    updateTimerDic.Add(action, timer);
                }
                else
                {
                    ReferencePool.Instance.Release(updateTimerDic[action]);
                    updateTimerDic[action] = timer;
                }
            }
            else
            {
                if (!addTimerDic.ContainsKey(action))
                {
                    timer = Timer.CreateTimer(interval, repeat, OnComplete);
                    timer.Calculate(elapsedTime);
                    addTimerDic.Add(action, timer);
                }

                if (removeTimerSet.Contains(action))
                {
                    removeTimerSet.Remove(action);
                }
            }
        }

        public void AddTimer(float delay, float interval, int repeat, System.Action action, System.Action OnComplete = null)
        {
            Timer timer = null;
            if (!isInUpdate)
            {
                timer = Timer.CreateTimer(delay, interval, repeat, OnComplete);
                timer.Calculate(elapsedTime, true);
                if (!updateTimerDic.ContainsKey(action))
                {
                    updateTimerDic.Add(action, timer);
                }
                else
                {
                    ReferencePool.Instance.Release(updateTimerDic[action]);
                    updateTimerDic[action] = timer;
                }
            }
            else
            {
                if (!addTimerDic.ContainsKey(action))
                {
                    timer = Timer.CreateTimer(delay, interval, repeat, OnComplete);
                    timer.Calculate(elapsedTime, true);
                    addTimerDic.Add(action, timer);
                }

                if (removeTimerSet.Contains(action))
                {
                    removeTimerSet.Remove(action);
                }

            }
        }

        public void RemoveTimer(System.Action action)
        {
            if (!isInUpdate)
            {
                if (updateTimerDic.ContainsKey(action))
                {
                    updateTimerDic[action].OnTimerComplete?.Invoke();
                    ReferencePool.Instance.Release(updateTimerDic[action]);
                    updateTimerDic.Remove(action);
                }
            }
            else
            {
                if (addTimerDic.ContainsKey(action))
                {
                    ReferencePool.Instance.Release(addTimerDic[action]);
                    addTimerDic.Remove(action);
                }
                //if (!removeTimerSet.Contains(action))
                //{
                //    removeTimerSet.Add(action);
                //}
                if (updateTimerDic.ContainsKey(action))
                {
                    removeTimerSet.Add(action);
                }
            }
        }
        public bool HasTimer(System.Action action)
        {
            if (!isInUpdate)
            {
                return updateTimerDic.ContainsKey(action);
            }
            else
            {
                if (removeTimerSet.Contains(action))
                {
                    return false;
                }
                else if (addTimerDic.ContainsKey(action))
                {
                    return true;
                }
                else
                {
                    return updateTimerDic.ContainsKey(action);
                }
            }
        }
        public void AddObserver(System.Action action)
        {
            if (!isInUpdate)
            {
                updateObservers.Add(action);
            }
            else
            {
                if (!addObservers.Contains(action))
                {
                    addObservers.Add(action);
                }
                if (removeObservers.Contains(action))
                {
                    removeObservers.Remove(action);
                }
            }
        }

        public void RemoveObserver(System.Action action)
        {
            if (!isInUpdate)
            {
                updateObservers.Remove(action);
            }
            else
            {
                if (addObservers.Contains(action))
                {
                    addObservers.Remove(action);
                }
                if (!removeObservers.Contains(action))
                {
                    removeObservers.Add(action);
                }
            }
        }

        public void OnUpdate(float deltaTime)
        {

            elapsedTime += deltaTime;
            isInUpdate = true;

            //更新计时器
            foreach (var keyValuePair in updateTimerDic)
            {
                System.Action action = keyValuePair.Key;
                Timer timer = keyValuePair.Value;

                if (removeTimerSet.Contains(action))     //如果已经在移除缓冲，不更新计时器
                {
                    continue;
                }

                if (timer.scheduledTime <= elapsedTime)
                {
                    if (timer.repeat == 0)
                    {
                        //ReferencePool.Instance.Release(timer);
                        RemoveTimer(action);
                    }
                    else if (timer.repeat >= 0)
                    {
                        timer.repeat--;
                    }
                    action?.Invoke();
                    timer.Calculate(elapsedTime);
                }
            }

            foreach (var action in updateObservers)
            {
                action?.Invoke();
            }

            //进行添加计时器
            if (addTimerDic.Count > 0)
            {
                foreach (var action in addTimerDic.Keys)
                {
                    if (updateTimerDic.ContainsKey(action))
                    {
                        ReferencePool.Instance.Release(updateTimerDic[action]);
                        updateTimerDic[action] = addTimerDic[action];

                    }
                    else
                    {
                        updateTimerDic.Add(action, addTimerDic[action]);
                    }
                }
            }

            if (addObservers.Count > 0)
            {
                foreach (var action in addObservers)
                {
                    updateObservers.Add(action);
                }
            }

            //进行移除计时器
            if (removeTimerSet.Count > 0)
            {
                foreach (var action in removeTimerSet)
                {
                    if (updateTimerDic.ContainsKey(action))
                    {
                        updateTimerDic[action].OnTimerComplete?.Invoke();
                        ReferencePool.Instance.Release(updateTimerDic[action]);
                        updateTimerDic.Remove(action);
                    }
                }
            }

            if (removeObservers.Count > 0)
            {
                foreach (var action in removeObservers)
                {
                    updateObservers.Remove(action);
                }
            }

            //清空缓冲容器
            addTimerDic.Clear();
            removeTimerSet.Clear();
            addObservers.Clear();
            removeObservers.Clear();

            isInUpdate = false;

        }
    }
}



