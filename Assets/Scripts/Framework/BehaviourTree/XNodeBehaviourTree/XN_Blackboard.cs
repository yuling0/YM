using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTreeEditor
{
    public enum E_ChangeType
    {
        ADD,
        REMOVE,
        MODIFY
    }
    public class XN_Blackboard
    {
        Dictionary<string, ISharedType> dataDir = new Dictionary<string, ISharedType>();  //可能用装拆箱
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> updateObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> addObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> removeObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        List<NotifyInfo> notifyInfoList = new List<NotifyInfo>();
        XN_GlobalClock clock;

        bool isNotifying = false;
        private struct NotifyInfo
        {
            public E_ChangeType type;
            public string key;
            public ISharedType data;
            public NotifyInfo(E_ChangeType type , string key , ISharedType data)
            {
                this.type = type;
                this.key = key;
                this.data = data;
            }
        }
        public XN_Blackboard (XN_GlobalClock clock)
        {
            this.clock = clock;
        }

        public ISharedType this[string key]
        {
            get
            {
                if (dataDir.TryGetValue(key, out var data))
                {
                    return data;
                }
                return default;
            }
            set
            {
                SetKey(key, value);
            }
        }

        public bool IsSet(string key)
        {
            return dataDir.ContainsKey(key);
        }
        public void SetKey<T>(string key, T val) where T : ISharedType
        {
            Type type = typeof(T);
            if (!dataDir.ContainsKey(key))   //黑板中没有此key对应的数据 执行添加操作
            {
                dataDir[key] = val;

                notifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, val));

                clock.AddTimer(0, 0, NotifyObservers);
            }
            else
            {
                if ((dataDir[key] == null && val != null) || (dataDir[key] != null && !dataDir[key].Equals(val)))    //判断是否修改了数据
                {
                    dataDir[key] = val;

                    notifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, val));

                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public T GetKey<T>(string key) where T : ISharedType
        {
            if (dataDir.TryGetValue(key, out var data))
            {
                
                if (!(data is T))
                {
                    Debug.LogError(string.Format("黑板中key为{0}的数据 不是指定{1}的数据", key, typeof(T).Name));
                    return default;
                }
                return (T)data;
            }

            return default;
        }

        public void AddObserver(string key,Action<E_ChangeType, ISharedType> action)
        {
            if(!isNotifying)
            {
                List<Action<E_ChangeType, ISharedType>> observers = GetObserverList(updateObserverDic, key);
                
                if(!observers.Contains(action))
                {
                    observers.Add(action);
                }
            }
            else
            {
                List<Action<E_ChangeType, ISharedType>> observers = GetObserverList(addObserverDic, key);

                if (!observers.Contains(action))       //如果不在添加缓冲中，则添加
                {
                    observers.Add(action);
                }

                observers = GetObserverList(removeObserverDic, key);

                if(observers.Contains(action))         //如果在移除缓冲中，则将这委托从移除缓冲移除（即这委托是要添加到updateObserverDic中）
                {
                    observers.Remove(action);
                }
            }
        }

        public void RemoveObserver(string key, Action<E_ChangeType, ISharedType> action)
        {
            if (!isNotifying)
            {
                List<Action<E_ChangeType, ISharedType>> observers = GetObserverList(updateObserverDic, key);

                if (observers.Contains(action))
                {
                    observers.Remove(action);
                }
            }
            else
            {
                List<Action<E_ChangeType, ISharedType>> observers = GetObserverList(addObserverDic, key);

                if (observers.Contains(action))       
                {
                    observers.Remove(action);
                }

                observers = GetObserverList(removeObserverDic, key);

                if (!observers.Contains(action))         
                {
                    observers.Add(action);
                }
            }
        }


        /// <summary>
        /// 通知观察者们
        /// </summary>
        private void NotifyObservers()
        {
            isNotifying = true;


            foreach (NotifyInfo notifyInfo in notifyInfoList)
            {
                if(!updateObserverDic.ContainsKey(notifyInfo.key))
                {
                    continue;
                }

                List<Action<E_ChangeType, ISharedType>> observerList = GetObserverList(updateObserverDic, notifyInfo.key);
                foreach (var action in observerList)
                {
                    action?.Invoke(notifyInfo.type, notifyInfo.data);
                }
            }

            foreach (var key in addObserverDic.Keys)
            {
                GetObserverList(updateObserverDic, key).AddRange(addObserverDic[key]);
                addObserverDic[key].Clear();
            }

            foreach (var key in removeObserverDic.Keys)
            {
                List<Action<E_ChangeType, ISharedType>> observerList = GetObserverList(updateObserverDic, key);

                foreach (var action in removeObserverDic[key])
                {
                    observerList.Remove(action);
                }

                removeObserverDic[key].Clear();
            }

            addObserverDic.Clear();
            removeObserverDic.Clear();
            notifyInfoList.Clear();

            isNotifying = false;
        }


        private List<Action<E_ChangeType, ISharedType>> GetObserverList(Dictionary<string, List<Action<E_ChangeType, ISharedType>>> dic, string key)
        {
            List<Action<E_ChangeType, ISharedType>> observers = null;
            if(!dic.ContainsKey(key))
            {
                dic[key] = new List<Action<E_ChangeType, ISharedType>>();
            }
            observers = dic[key];
            return observers;
        }
    }
}

