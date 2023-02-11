using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public delegate T DoGetter<out T>();
public delegate void DoSetter<in T>(T val);
public class TweenManager : SingletonBase<TweenManager>
{
    private Dictionary<object, List<Tween>> dic = new Dictionary<object, List<Tween>>();
    private HashSet<Tween> updateTweeners = new HashSet<Tween>();
    private HashSet<Tween> addTweeners = new HashSet<Tween>();
    private HashSet<Tween> removeTweeners = new HashSet<Tween>();
    private bool isInUpdate;
    private TweenManager()
    {
        MonoMgr.Instance.AddUpateAction(OnUpdate);
    }
    //TODO:缓存Tween没写完，应该提供一个清空缓存的方法（比如过场景时清空）
    public void CacheTweener(Tween tweener)
    {
        object target = tweener.target;
        if (target == null)
        {
            Debug.Log($"该补间动画：{tweener}的target为null");
            return;
        }
        if (!dic.ContainsKey(target))
        {
            dic.Add(target, new List<Tween>());
        }
        dic[target].Add(tweener);
    }
    public void AddTweener(Tween tweener)
    {

        if (isInUpdate)
        {
            if (!addTweeners.Contains(tweener))
            {
                addTweeners.Add(tweener);
            }
            if (removeTweeners.Contains(tweener))
            {
                removeTweeners.Remove(tweener);
            }
        }
        else
        {
            if (!updateTweeners.Contains(tweener))
            {
                updateTweeners.Add(tweener);
            }
            
        }

    }

    public void RemoveTweener(Tween tweener)
    {
        if (isInUpdate)
        {

            if (!removeTweeners.Contains(tweener))
            {
                removeTweeners.Add(tweener);
            }

            if (addTweeners.Contains(tweener))
            {
                addTweeners.Remove(tweener);
            }
        }
        else
        {
            if (updateTweeners.Contains(tweener))
            {
                updateTweeners.Remove(tweener);
            }

        }
    }
    public void KillTweener(Tween tweener)
    {
        object target = tweener.target;
        if (target != null && dic.ContainsKey(target))
        {
            if (dic[target].Contains(tweener))
            {
                dic[target].Remove(tweener);
            }
        }
    }
    public static TweenerSequence Sequence()
    {
        TweenerSequence sequence = ReferencePool.Instance.Acquire<TweenerSequence>();
        Instance.AddTweener(sequence);
        sequence.state = E_TweenState.InActive;
        return sequence;
    }
    public static Tween To(DoGetter<int> getter , DoSetter<int> setter ,int startValue, int endValue , float duration)
    {
        IntTweener intTweener = IntTweener.Create();
        intTweener.getter = getter;
        intTweener.setter = setter;
        intTweener.SetParameter(startValue, endValue, duration);
        Instance.AddTweener(intTweener);
        return intTweener;
    }
    public static Tween To(DoGetter<float> getter, DoSetter<float> setter, float startValue, float endValue, float duration)
    {
        FloatTweener intTweener = FloatTweener.Create();
        intTweener.getter = getter;
        intTweener.setter = setter;
        intTweener.SetParameter(startValue, endValue, duration);
        Instance.AddTweener(intTweener);
        return intTweener;
    }
    public static Tween To(DoGetter<string> getter, DoSetter<string> setter, string endValue, float duration)
    {
        StringTweener textTween = StringTweener.Create();
        textTween.getter = getter;
        textTween.setter = setter;
        textTween.SetParameter(endValue, duration);
        Instance.AddTweener(textTween);
        return textTween;
    }
    public static Tween To(DoGetter<string> getter, DoSetter<string> setter, string startValue, string endValue, float duration)
    {
        StringTweener textTween = StringTweener.Create();
        textTween.getter = getter;
        textTween.setter = setter;
        textTween.SetParameter(startValue, endValue, duration);
        Instance.AddTweener(textTween);
        return textTween;
    }
    public static Tween To(DoGetter<Color> getter, DoSetter<Color> setter, float endValue, float duration)
    {
        ColorAlphaTweener colorAlphaTweener = ColorAlphaTweener.Create();
        colorAlphaTweener.setter = setter;
        colorAlphaTweener.getter = getter;
        colorAlphaTweener.SetParameter(endValue, duration);
        Instance.AddTweener(colorAlphaTweener);
        return colorAlphaTweener;
    }
    public static Tween To(DoGetter<Color> getter, DoSetter<Color> setter, float startValue, float endValue, float duration)
    {
        ColorAlphaTweener colorAlphaTweener = ColorAlphaTweener.Create();
        colorAlphaTweener.setter = setter;
        colorAlphaTweener.getter = getter;
        colorAlphaTweener.SetParameter(startValue, endValue, duration);
        Instance.AddTweener(colorAlphaTweener);
        return colorAlphaTweener;
    }
    public static Tween To(DoGetter<Vector2> getter , DoSetter<Vector2> setter , Vector2 endValue , float duration)
    {
        Vector2Tweener vector2Tweener = Vector2Tweener.Create();
        vector2Tweener.setter = setter;
        vector2Tweener.getter = getter;
        vector2Tweener.SetParameter(endValue, duration);
        Instance.AddTweener(vector2Tweener);
        return vector2Tweener;
    }

    public static Tween To(DoGetter<Vector2> getter, DoSetter<Vector2> setter, Vector2 startValue, Vector2 endValue, float duration)
    {
        Vector2Tweener vector2Tweener = Vector2Tweener.Create();
        vector2Tweener.setter = setter;
        vector2Tweener.getter = getter;
        vector2Tweener.SetParameter(startValue ,endValue, duration);
        Instance.AddTweener(vector2Tweener);
        return vector2Tweener;
    }

    public static Tween To(DoGetter<Vector3> getter, DoSetter<Vector3> setter, Vector3 endValue, float duration)
    {
        Vector3Tweener vector3Tweener = Vector3Tweener.Create();
        vector3Tweener.setter = setter;
        vector3Tweener.getter = getter;
        vector3Tweener.SetParameter(endValue, duration);
        Instance.AddTweener(vector3Tweener);
        return vector3Tweener;
    }

    public static Tween To(DoGetter<Vector3> getter, DoSetter<Vector3> setter , Vector3 startValue, Vector3 endValue, float duration)
    {
        Vector3Tweener vector3Tweener = Vector3Tweener.Create();
        vector3Tweener.setter = setter;
        vector3Tweener.getter = getter;
        vector3Tweener.SetParameter(startValue, endValue, duration);
        Instance.AddTweener(vector3Tweener);
        return vector3Tweener;
    }
    private void OnUpdate()
    {
        isInUpdate = true;

        foreach (var tweener in updateTweeners)
        {
            if (removeTweeners.Contains(tweener))
            {
                continue;
            }
            if (tweener.IsComplete && tweener.IsAutoKill)
            {
                tweener.Kill();
                RemoveTweener(tweener);
                continue;
            }
            else if(tweener.IsComplete || tweener.IsInSequence) //如果补间动画已完成或补间动画在Sequence中，移除更新容器
            {
                RemoveTweener(tweener);
                continue;
            }
            else if (tweener.IsInActive)
            {
                tweener.Restart();
            }
            tweener.UpdateTween();
            Debug.Log(tweener.GetType().Name);
        }
        Debug.Log(updateTweeners.Count);
        if (addTweeners.Count > 0)
        {
            foreach (var tweener in addTweeners)
            {
                if (!updateTweeners.Contains(tweener))
                {
                    updateTweeners.Add(tweener);
                }
            }
        }

        if (removeTweeners.Count > 0)
        {
            foreach (var tweener in removeTweeners)
            {
                if(updateTweeners.Contains(tweener))
                {
                    updateTweeners.Remove(tweener);
                }
                else
                {
                    Debug.Log("updateTweeners中没有指定需要移除的tweener");
                }
            }
        }
        addTweeners.Clear();
        removeTweeners.Clear();
        isInUpdate = false;
    }
}
