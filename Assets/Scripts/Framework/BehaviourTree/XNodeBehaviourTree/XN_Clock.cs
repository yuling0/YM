using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YMFramework.BehaviorTreeEditor
{
    public partial class XN_GlobalClock
    {
        private class XN_Clock 
        {

            Dictionary<UnityAction, Timer> updateTimerDic = new Dictionary<UnityAction, Timer>();       //计时器容器

            Dictionary<UnityAction, Timer> addTimerDic = new Dictionary<UnityAction, Timer>();    //添加缓冲容器
            HashSet<UnityAction> removeTimerSet = new HashSet<UnityAction>();     //移除缓冲容器

            List<UnityAction> updateObservers = new List<UnityAction>();          //需要持续更新的委托（即没有delay时间）
            HashSet<UnityAction> addObservers = new HashSet<UnityAction>();       //添加缓冲
            HashSet<UnityAction> removeObservers = new HashSet<UnityAction>();    //移除缓冲


            bool isInUpdate = false;        //是否正在更新（即是否正在遍历容器，正在更新时，不能更改容器，只能添加至缓冲区）
            float elapsedTime = 0f;         //当前运行时间
            private class Timer:IReference
            {
                public float scheduledTime;  //预计执行的时间
                public float delay;          //延迟时间
                public int repeat;           //重复次数

                public static Timer CreateTimer(float delay , int repeat)
                {
                    Timer t = ReferencePool.Instance.Acquire<Timer>();
                    t.delay = delay;
                    t.repeat = repeat;
                    return t;
                }
                public void Calculate(float elapsedTime)
                {
                    scheduledTime = elapsedTime + delay;
                }

                public void Clear()
                {
                    scheduledTime = 0f;
                    delay = 0f;
                    repeat = 0;
                }
            }

            /// <summary>
            /// 添加一个计时器
            /// </summary>
            /// <param name="delay"></param>
            /// <param name="repeat"></param>
            /// <param name="action"></param>
            public void AddTimer(float delay, int repeat, UnityAction action)
            {
                Timer timer = null;
                if (!isInUpdate)
                {
                    
                    if (!updateTimerDic.ContainsKey(action))
                    {
                        timer = Timer.CreateTimer(delay, repeat);
                        timer.Calculate(elapsedTime);
                        updateTimerDic.Add(action, timer);
                    }
                    else
                    {
                        updateTimerDic[action].Clear();
                        updateTimerDic[action] = timer;
                    }
                }
                else
                {
                    if(!addTimerDic.ContainsKey(action))
                    {
                        timer = Timer.CreateTimer(delay, repeat);
                        timer.Calculate(elapsedTime);
                        addTimerDic.Add(action,timer);
                    }

                    if (removeTimerSet.Contains(action))
                    {
                        removeTimerSet.Remove(action);
                    }
                }
            }

            public void RemoveTimer(UnityAction action)
            {
                if(!isInUpdate)
                {
                    if(updateTimerDic.ContainsKey(action))
                    {
                        updateTimerDic[action].Clear();
                        updateTimerDic.Remove(action);
                    }
                }
                else
                {
                    if(addTimerDic.ContainsKey(action))
                    {
                        addTimerDic[action].Clear();
                        addTimerDic.Remove(action);
                    }
                    if(!removeTimerSet.Contains(action))
                    {
                        removeTimerSet.Add(action);
                    }
                }
            }

            public void AddObserver(UnityAction action)
            {
                if (!isInUpdate)
                {
                    updateObservers.Add(action);
                }
                else
                {
                    if(!addObservers.Contains(action))
                    {
                        addObservers.Add(action);
                    }
                    if(removeObservers.Contains(action))
                    {
                        removeObservers.Remove(action);
                    }
                }
            }

            public void RemoveObserver(UnityAction action)
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
                    UnityAction action = keyValuePair.Key;
                    Timer timer = keyValuePair.Value;

                    if (removeTimerSet.Contains(action))     //如果已经在移除缓冲，不更新计时器
                    {
                        continue;
                    }

                    if (timer.scheduledTime <= elapsedTime)
                    {
                        
                        if(timer.repeat == 0)
                        {
                            timer.Clear();
                            RemoveTimer(action);
                        }
                        else
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
                        if(updateTimerDic.ContainsKey(action))
                        {
                            updateTimerDic[action].Clear();
                            updateTimerDic[action] = addTimerDic[action];

                        }
                        else
                        {
                            updateTimerDic.Add(action, addTimerDic[action]);
                        }
                    }
                }

                if(addObservers.Count > 0)
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
                            updateTimerDic[action].Clear();
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

}

