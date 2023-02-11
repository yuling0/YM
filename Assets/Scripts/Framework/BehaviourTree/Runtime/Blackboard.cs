using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public enum E_ChangeType
    {
        ADD,
        REMOVE,
        MODIFY,
        TRIGGER
    }
    public class Blackboard
    {
        Dictionary<string, ISharedType> dataDir = new Dictionary<string, ISharedType>(); 
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> updateObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> addObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        Dictionary<string, List<Action<E_ChangeType, ISharedType>>> removeObserverDic = new Dictionary<string, List<Action<E_ChangeType, ISharedType>>>();
        List<NotifyInfo> updateNotifyInfoList = new List<NotifyInfo>();
        List<NotifyInfo> addNotifyInfoList = new List<NotifyInfo>();
        GlobalClock clock;

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
        public Blackboard (GlobalClock clock)
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

        //TODO:后续添加设置对应类型值的方法（使用引用池，减少类生成）
        public void SetKey<T>(string key, T val) where T : ISharedType
        {
            Type type = typeof(T);
            if (!dataDir.ContainsKey(key))   //黑板中没有此key对应的数据 执行添加操作
            {
                dataDir[key] = val;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, val));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, val));
                }

                clock.AddTimer(0, 0, NotifyObservers);
            }
            else
            {
                if ((dataDir[key] == null && val != null) || (dataDir[key] != null && !dataDir[key].Equals(val)))    //判断是否修改了数据
                {
                    dataDir[key] = val;

                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, val));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, val));
                    }

                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public void SetBool(string key , bool val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedBool sharedBool = SharedBool.CreateSharedBool();
                sharedBool.val = val;
                dataDir[key] = sharedBool;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedBool));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedBool));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedBool sharedBool = dataDir[key] as SharedBool;
                if (sharedBool == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedBool == null || !sharedBool.val.Equals(val))
                {
                    if (sharedBool == null)
                    {
                        sharedBool = SharedBool.CreateSharedBool();
                        sharedBool.val = val;
                        dataDir[key] = sharedBool;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedBool);
                        sharedBool = SharedBool.CreateSharedBool();
                        sharedBool.val = val;
                        dataDir[key] = sharedBool;
                    }

                    
                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public void SetFloat(string key, float val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedFloat sharedFloat = SharedFloat.CreateSharedFloat();
                sharedFloat.val = val;
                dataDir[key] = sharedFloat;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedFloat));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedFloat));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedFloat sharedFloat = dataDir[key] as SharedFloat;
                if (sharedFloat == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedFloat == null || !sharedFloat.val.Equals(val))
                {
                    if (sharedFloat == null)
                    {
                        sharedFloat = SharedFloat.CreateSharedFloat();
                        sharedFloat.val = val;
                        dataDir[key] = sharedFloat;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedFloat);
                        sharedFloat = SharedFloat.CreateSharedFloat();
                        sharedFloat.val = val;
                        dataDir[key] = sharedFloat;
                    }


                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public void SetInt(string key, int val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedInt sharedInt = SharedInt.CreateSharedInt();
                sharedInt.val = val;
                dataDir[key] = sharedInt;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedInt));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedInt));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedInt sharedInt = dataDir[key] as SharedInt;
                if (sharedInt == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedInt == null || !sharedInt.val.Equals(val))
                {
                    if (sharedInt == null)
                    {
                        sharedInt = SharedInt.CreateSharedInt();
                        sharedInt.val = val;
                        dataDir[key] = sharedInt;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedInt);
                        sharedInt = SharedInt.CreateSharedInt();
                        sharedInt.val = val;
                        dataDir[key] = sharedInt;
                    }


                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }


        public void SetVector2(string key, Vector2 val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedVector2 sharedVector2 = SharedVector2.CreateSharedVector2();
                sharedVector2.val = val;
                dataDir[key] = sharedVector2;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedVector2));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedVector2));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedVector2 sharedVector2 = dataDir[key] as SharedVector2;
                if (sharedVector2 == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedVector2 == null || !sharedVector2.val.Equals(val))
                {
                    if (sharedVector2 == null)
                    {
                        sharedVector2 = SharedVector2.CreateSharedVector2();
                        sharedVector2.val = val;
                        dataDir[key] = sharedVector2;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedVector2);
                        sharedVector2 = SharedVector2.CreateSharedVector2();
                        sharedVector2.val = val;
                        dataDir[key] = sharedVector2;
                    }


                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public void SetVector3(string key, Vector3 val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedVector3 sharedVector3 = SharedVector3.CreateSharedVector3();
                sharedVector3.val = val;
                dataDir[key] = sharedVector3;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedVector3));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedVector3));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedVector3 sharedVector3 = dataDir[key] as SharedVector3;
                if (sharedVector3 == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedVector3 == null || !sharedVector3.val.Equals(val))
                {
                    if (sharedVector3 == null)
                    {
                        sharedVector3 = SharedVector3.CreateSharedVector3();
                        sharedVector3.val = val;
                        dataDir[key] = sharedVector3;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedVector3);
                        sharedVector3 = SharedVector3.CreateSharedVector3();
                        sharedVector3.val = val;
                        dataDir[key] = sharedVector3;
                    }


                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


                    clock.AddTimer(0, 0, NotifyObservers);
                }
            }
        }

        public void SetString(string key, string val)
        {
            if (!dataDir.ContainsKey(key))      //不包含这个数据，添加数据
            {
                SharedString sharedString = SharedString.CreateSharedString();
                sharedString.val = val;
                dataDir[key] = sharedString;

                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedString));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.ADD, key, sharedString));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }
            else        //包含这个数据，判断是否修改，若修改了，则通知观察者
            {
                SharedString sharedString = dataDir[key] as SharedString;
                if (sharedString == null)
                {
                    Debug.Log($"键为：{key}的值为null 或者 不是SharedBool类型");
                }

                if (sharedString == null || !sharedString.val.Equals(val))
                {
                    if (sharedString == null)
                    {
                        sharedString = SharedString.CreateSharedString();
                        sharedString.val = val;
                        dataDir[key] = sharedString;
                    }
                    else
                    {
                        ReferencePool.Instance.Release(sharedString);
                        sharedString = SharedString.CreateSharedString();
                        sharedString.val = val;
                        dataDir[key] = sharedString;
                    }


                    if (!isNotifying)
                    {
                        updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }
                    else
                    {
                        addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.MODIFY, key, dataDir[key]));
                    }


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

        public void SetTrigger(string key)
        {
            if (!dataDir.ContainsKey(key))
            {
                dataDir.Add(key, new SharedBool());
            }

            if (dataDir[key] is SharedBool && !(dataDir[key] as SharedBool).val)
            {
                (dataDir[key] as SharedBool).val = true;
                if (!isNotifying)
                {
                    updateNotifyInfoList.Add(new NotifyInfo(E_ChangeType.TRIGGER, key, dataDir[key]));
                }
                else
                {
                    addNotifyInfoList.Add(new NotifyInfo(E_ChangeType.TRIGGER, key, dataDir[key]));
                }
                clock.AddTimer(0, 0, NotifyObservers);
            }

        }

        public bool IsTrigger(string key)
        {
            if (dataDir.ContainsKey(key) && dataDir[key] is SharedBool)
            {
                return (dataDir[key] as SharedBool).val;
            }
            return false;
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


            foreach (NotifyInfo notifyInfo in updateNotifyInfoList)
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

                //触发器触发完成
                if (notifyInfo.type == E_ChangeType.TRIGGER)
                {
                    (notifyInfo.data as SharedBool).val = false;
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
            updateNotifyInfoList.Clear();

            updateNotifyInfoList.AddRange(addNotifyInfoList);
            addNotifyInfoList.Clear();

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

