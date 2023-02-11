using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenerSequence : Tween
{
    #region 补间列表容器
    class TweenerList : IReference
    {
        public bool isActive;
        public int completeCount = 0;
        public float beginTime; 
        public List<Tween> tweeners = new List<Tween>();
        public Action onComplete;

        public static TweenerList Create()
        {
            return ReferencePool.Instance.Acquire<TweenerList>();
        }
        public void AddTweener(Tween tweener)
        {
            tweeners.Add(tweener);
            tweener.Pause();
            tweener.SetAutoKill(false);
            tweener.OnComplete(() => 
            { 
                completeCount++;
                if (completeCount == tweeners.Count)
                {
                    onComplete?.Invoke();
                    isActive = false;
                }
            });
        }

        public void OnUpdate()
        {
            foreach (var tweener in tweeners)
            {
                tweener.UpdateTween();
            }
        }
        public void Restart()
        {
            isActive = true;
            completeCount = 0;
            foreach (var tweener in tweeners)
            {
                tweener.Restart();
            }
        }

        public void Kill()
        {
            onComplete = null;
            for (int i = 0; i < tweeners.Count; i++)
            {
                tweeners[i].Kill();
            }
            ReferencePool.Instance.Release(this);
        }
        public void SetUpdate(bool isUnScale)
        {
            foreach (var tweener in tweeners)
            {
                tweener.SetUpdate(isUnScale);
            }
        }
        public void Clear()
        {
            completeCount = 0;
            onComplete = null;
            tweeners.Clear();
            beginTime = 0f;
            isActive = false;
        }
    }
    #endregion
    private float beginDelayTime;
    private float endDelayTime;

    private LinkedList<TweenerList> normalSequence = new LinkedList<TweenerList>();     //根据普通先后顺序执行的序列
    private LinkedList<TweenerList> timeSequence = new LinkedList<TweenerList>();       //根据时间顺序执行的序列
    private LinkedListNode<TweenerList> normalSequenceNode;                             //当前节点（普通顺序）
    private LinkedListNode<TweenerList> timeSequenceNode;                               //当前节点（时间顺序）
    private int timeSequenceNodeCompleteCount;                                          //节点的完成数量（时间顺序）
    private bool isNormalSequenceComplete;       //normalSequence是否全部播放完成
    private bool isTimeSequenceComplete;         //timeSequence 是否全部播放完成
    public override void Restart()
    {
        base.Restart();
        isNormalSequenceComplete = false;
        isTimeSequenceComplete = false;
        TweenManager.Instance.AddTweener(this);         //这里应变多次播放这个序列（因为播放后不自动杀死的动画，播放完成后会移出更新容器）
        if (normalSequence.Count > 0)
        {
            normalSequenceNode = normalSequence.First;
            normalSequenceNode.Value.Restart();
        }
        else
        {
            isNormalSequenceComplete = true;
        }
        if (timeSequence.Count > 0)
        {
            timeSequenceNode = timeSequence.First;
        }
        else
        {
            isTimeSequenceComplete = true;
        }
        CheckIsSequenceComplete();
    }
    public override void UpdateTween()
    {
        if (!IsPlaying) return;

        timer += ScaleType == E_TimeScaleType.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

        if (normalSequenceNode != null && normalSequenceNode.Value.isActive)
        {
            normalSequenceNode.Value.OnUpdate();
        }

        while (timeSequenceNode != null && timeSequenceNode.Value.beginTime + beginDelayTime <= timer)
        {
            timeSequenceNode.Value.Restart();
            timeSequenceNode = timeSequenceNode.Next;
        }
        foreach (var tweenerList in timeSequence)
        {
            if (tweenerList.isActive)
            {
                tweenerList.OnUpdate();
            }
            else
            {
                break;
            }
        }

    }
    public override void Kill()
    {
        Pause();
        foreach (var tweenerList in normalSequence)
        {
            tweenerList.Kill();
        }
        TweenManager.Instance.RemoveTweener(this);
        ReferencePool.Instance.Release(this);

    }
    public override Tween SetUpdate(bool isUnScale)
    {
        foreach (var tweenerList in normalSequence)
        {
            tweenerList.SetUpdate(isUnScale);
        }
        foreach (var tweenerList in timeSequence)
        {
            tweenerList.SetUpdate(isUnScale);
        }
        return base.SetUpdate(isUnScale);
    }

    public override void Clear()
    {
        base.Clear();
        normalSequence.Clear();
        timeSequence.Clear();
        normalSequenceNode = null;
        timeSequenceNode = null;
        timeSequenceNodeCompleteCount = 0;
        isNormalSequenceComplete = false;
        isTimeSequenceComplete = false;
    }
    public TweenerSequence AddBeginDelayTime(float second)
    {
        beginDelayTime += second;
        if (beginDelayTime < 0f)
        {
            beginDelayTime = 0f;
        }
        return this;
    }

    public TweenerSequence AddEndDelayTime(float second)
    {
        endDelayTime += second;
        if (endDelayTime < 0f)
        {
            endDelayTime = 0f;
        }
        return this;
    }
    /// <summary>
    /// 添加一个补间动画（在上一个动画播放之后，紧接着播放）
    /// </summary>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public TweenerSequence Append(Tween tweener)
    {
        tweener.Pause();
        tweener.IsInSequence = true;
        tweener.SetUpdate(ScaleType == E_TimeScaleType.Normal ? false : true);
        TweenerList tweenerList = TweenerList.Create();
        tweenerList.AddTweener(tweener);
        tweenerList.onComplete += OnNormalTweenerListComplete;
        normalSequence.AddLast(tweenerList);
        return this;
    }
    /// <summary>
    /// 添加一个时间间隔
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public TweenerSequence AppendInterval(float second)
    {
        TweenerList tweenerList = TweenerList.Create();
        IntervalTweener intervalTweener = IntervalTweener.Create();
        intervalTweener.SetUpdate(ScaleType == E_TimeScaleType.Normal ? false : true);
        intervalTweener.SetDuration(second);
        intervalTweener.IsInSequence = true;
        tweenerList.AddTweener(intervalTweener);
        tweenerList.onComplete += OnNormalTweenerListComplete;
        normalSequence.AddLast(tweenerList);
        return this;
    }
    /// <summary>
    /// 加入到上一个补间动画的序列中（说人话：跟上一个动画同时播放）
    /// </summary>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public TweenerSequence Join(Tween tweener)
    {
        tweener.Pause();
        tweener.IsInSequence = true;
        tweener.SetUpdate(ScaleType == E_TimeScaleType.Normal ? false : true);
        if (normalSequence.Count > 0)
        {
            TweenerList tweenerList = normalSequence.Last.Value;
            tweenerList.AddTweener(tweener);
        }
        else
        {
            TweenerList tweenerList = new TweenerList();
            tweenerList.AddTweener(tweener);
            tweenerList.onComplete += OnNormalTweenerListComplete;
            normalSequence.AddLast(tweenerList);
        }
        return this;
    }
    /// <summary>
    /// 在指定时间点添加补间动画
    /// </summary>
    /// <param name="position"></param>
    /// <param name="tweener"></param>
    /// <returns></returns>
    public TweenerSequence Insert(float position , Tween tweener)
    {
        LinkedListNode<TweenerList> curNode = timeSequence.First;
        tweener.SetUpdate(ScaleType == E_TimeScaleType.Normal ? false : true);
        //找出第一个比position大于或等于的TweenerList
        while (curNode != null && curNode.Value.beginTime < position)
        {
            curNode = curNode.Next;
        }

        if (curNode == null) //没有比position的（直接创建新的，加入链表末尾）
        {
            TweenerList tweenerList = new TweenerList();
            tweenerList.AddTweener(tweener);
            tweenerList.beginTime = position;
            tweenerList.onComplete += OnTimeTweenerListComplete;
            timeSequence.AddLast(tweenerList);
            if (timeSequence.Count == 1)
            {
                timeSequenceNode = timeSequence.Last;
            }
        }
        else
        {
            if (curNode.Value.beginTime == position)    //时间点相同（直接加入当前）
            {
                curNode.Value.AddTweener(tweener);
            }
            else    //找到第一个大于的，创建一个新的TweenerList，加入到当前节点之前
            {
                TweenerList tweenerList = new TweenerList();
                tweenerList.AddTweener(tweener);
                tweenerList.beginTime = position;
                tweenerList.onComplete += OnTimeTweenerListComplete;
                timeSequence.AddBefore(curNode, tweenerList);
            }
        }
        return this;
    }

    /// <summary>
    /// 普通顺序序列播放完成回调
    /// </summary>
    private void OnNormalTweenerListComplete()
    {
        if (normalSequenceNode.Next != null)
        {
            normalSequenceNode = normalSequenceNode.Next;
            normalSequenceNode.Value.Restart();
        }
        else
        {
            isNormalSequenceComplete = true;
            CheckIsSequenceComplete();
        }
    }
    /// <summary>
    /// 时间顺序序列播放完成回调
    /// </summary>
    private void OnTimeTweenerListComplete()
    {
        timeSequenceNodeCompleteCount++;

        if (timeSequenceNodeCompleteCount == timeSequence.Count)
        {
            isTimeSequenceComplete = true;
            CheckIsSequenceComplete();
        }
    }

    private void CheckIsSequenceComplete()
    {
        if (isNormalSequenceComplete && isTimeSequenceComplete)
        {
            onComplete?.Invoke();
            state = E_TweenState.Complete;
            Debug.Log("序列播放完成");
        }
    }
}
