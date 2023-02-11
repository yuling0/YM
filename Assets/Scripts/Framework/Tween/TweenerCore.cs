using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public abstract class Tween : IReference
{
    public object target;
    public System.Action onComplete;
    public System.Action onUpdate;
    public E_TimeScaleType ScaleType = E_TimeScaleType.Normal;
    public E_TweenState state = E_TweenState.InActive;
    protected float timer = 0f;
    protected float duration;
    protected bool isAutoKill = true;
    protected bool isInSequence = false;

    public Tween OnComplete(System.Action onComplete)
    {
        this.onComplete += onComplete;
        return this;
    }
    public Tween OnUpdate(System.Action onUpdate)
    {
        this.onUpdate += onUpdate;
        return this;
    }
    public bool IsAutoKill
    {
        get => isAutoKill;
        set => isAutoKill = value;
    }
    public bool IsComplete => state == E_TweenState.Complete;
    public bool IsInActive => state == E_TweenState.InActive;
    public bool IsPlaying => state == E_TweenState.Playing;
    public bool IsPause => state == E_TweenState.Pause;
    public bool IsKilled => state == E_TweenState.Killed;
    public bool IsInSequence
    {
        get => isInSequence;
        set => isInSequence = value;
    }
    public virtual void UpdateTween()
    {
        if (!IsPlaying) return;
        timer += ScaleType == E_TimeScaleType.Normal ? Time.deltaTime : Time.unscaledDeltaTime;

        if (timer <= duration)
        {
            Do();
            onUpdate?.Invoke();
        }
        else
        {
            End();
            state = E_TweenState.Complete;
            onComplete?.Invoke();
        }
    }

    protected virtual void Do()
    {
        
    }

    protected virtual void End()
    {

    }
    public virtual void Restart() 
    {
        state = E_TweenState.Playing;
        timer = 0f;
    }
    public virtual void Pause() 
    {
        if (state == E_TweenState.InActive || state == E_TweenState.Playing)
        {
            state = E_TweenState.Pause;
        }
    }

    public virtual void Continue()
    {
        if (state == E_TweenState.Pause)
        {
            state = E_TweenState.Playing;
        }
    }
    public virtual void Kill()
    {
        if (!isInSequence)
        {
            TweenManager.Instance.RemoveTweener(this);
        }
        TweenManager.Instance.KillTweener(this);
        ReferencePool.Instance.Release(this);
    }
    public virtual Tween SetUpdate(bool isUnScale)
    {
        ScaleType = isUnScale ? E_TimeScaleType.UnScaled : E_TimeScaleType.Normal;
        return this;
    }
    public virtual void Clear() 
    {
        onComplete = null;
        isAutoKill = true;
        duration = 0f;
        target = null;
        isInSequence = false;
        state = E_TweenState.InActive;
    }
}
public class IntervalTweener : Tween
{
    public static IntervalTweener Create()
    {
        return ReferencePool.Instance.Acquire<IntervalTweener>();
    }

    public void SetDuration(float duration) => this.duration = duration;
}
public class TweenerCore<T1,T2,T3> : Tween
{
    protected T2 startValue;
    protected T3 endValue;

    public DoGetter<T1> getter;
    public DoSetter<T1> setter;
    public bool isSetStartValue;
    public void SetParameter(T2 startValue, T3 endValue , float duration)
    {
        this.startValue = startValue;
        this.endValue = endValue;
        this.duration = duration;
        isSetStartValue = true;
    }
    public void SetParameter(T3 endValue, float duration)
    {
        this.endValue = endValue;
        this.duration = duration;
        isSetStartValue = false;
    }
    public override void Clear()
    {
        base.Clear();
        startValue = default;
        endValue = default;
        getter = default;
        setter = default;
    }
}
public class IntTweener : TweenerCore<int, int, int>
{
    bool isPositive;
    float speed;
    float cur;
    public static IntTweener Create()
    {
        return ReferencePool.Instance.Acquire<IntTweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            setter(startValue);
        }
        else
        {
            startValue = getter();
        }
        int diff = endValue - startValue;
        isPositive = diff >= 0 ? true : false;
        speed = Mathf.Abs(diff) / duration;
        cur = 0f;
    }
    protected override void Do()
    {
        DoInt();
    }
    protected override void End()
    {
        setter(endValue);
    }
    private void DoInt()
    {
        if (isPositive)
        {
            cur = startValue + timer * speed;
            setter(Mathf.FloorToInt(cur));
        }
        else
        {
            cur = startValue + timer * -speed;
            setter(Mathf.FloorToInt(cur));
        }
    }
}

public class FloatTweener : TweenerCore<float, float, float>
{
    bool isPositive;
    float speed;
    float cur;
    public static FloatTweener Create()
    {
        return ReferencePool.Instance.Acquire<FloatTweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            setter(startValue);
        }
        else
        {
            startValue = getter();
        }
        float diff = endValue - startValue;
        isPositive = diff >= 0 ? true : false;
        speed = Mathf.Abs(diff) / duration;
        cur = 0f;
    }
    protected override void Do()
    {
        DoInt();
    }
    protected override void End()
    {
        setter(endValue);
    }
    private void DoInt()
    {
        if (isPositive)
        {
            cur = startValue + timer * speed;
            setter(cur);
        }
        else
        {
            cur = startValue + timer * -speed;
            setter(cur);
        }
    }
}
public class StringTweener : TweenerCore<string, string, string>
{
    StringBuilder stringBuilder = new StringBuilder();
    float moveIndexSpeed;       
    float modifyLengthSpeed;
    float idx1;
    float idx2;
    int startValueLength;
    int curIdx1;
    int curIdx2;
    public static StringTweener Create()
    {
        return ReferencePool.Instance.Acquire<StringTweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            setter(startValue);
        }
        else
        {
            startValue = getter();
        }
        stringBuilder.Clear();
        stringBuilder.Append(startValue);
        startValueLength = startValue.Length;
        moveIndexSpeed = endValue.Length / duration;
        int dif = startValue.Length - endValue.Length;
        if (dif > 0)
        {
            modifyLengthSpeed = dif / duration;
        }

        idx1 = 0f;
        idx2 = 0f;
        curIdx1 = 0;
        curIdx2 = 0;
    }
    protected override void Do()
    {
        DoText();
        if (modifyLengthSpeed != 0)
        {
            ModifyLenth();
        }
    }
    protected override void End()
    {
        setter(endValue);
    }
    private void DoText()
    {
        idx1 = moveIndexSpeed * timer;

        if (curIdx1 < startValueLength)  //当前索引小于初始字符串的长度时，覆盖字符串
        {
            while (curIdx1 < idx1)
            {
                stringBuilder[curIdx1] = endValue[curIdx1];
                curIdx1++;
            }
            
        }
        else            //超过时，添加字符串
        {
            while (curIdx1 < idx1)
            {
                stringBuilder.Append(endValue[curIdx1]);
                curIdx1++;
            }
        }

        setter(stringBuilder.ToString()); 
    }

    private void ModifyLenth()  //修改字符串长度（当目标字符串endValue 比 startValue 长度短时）
    {
        idx2 = modifyLengthSpeed * timer;

        while (curIdx2 < idx2)
        {
            stringBuilder.Remove(stringBuilder.Length - 1, 1);
            curIdx2++;
        }
        setter(stringBuilder.ToString());


    }

    public override void Clear()
    {
        base.Clear();
        stringBuilder.Clear();
        modifyLengthSpeed = 0f;
    }

}
public class AddTextTweener : TweenerCore<string,string,string>
{
    float speed;
    StringBuilder sb = new StringBuilder();
    int index;
    System.Action onAddText;

    public AddTextTweener OnAddText(System.Action action)
    {
        onAddText += action;
        return this;
    }
    public static AddTextTweener Create()
    {
        return ReferencePool.Instance.Acquire<AddTextTweener>();
    }
    public override void Restart()
    {
        base.Restart();
        sb.Clear();
        if (isSetStartValue)
        {
            setter(startValue);

        }
        else
        {
            startValue = getter();
        }
        int len = endValue.Length;
        speed = len / duration;
        index = 0;
        sb.Append(startValue);

    }
    protected override void Do()
    {
        AddText();
    }
    protected override void End()
    {
        setter(startValue + endValue);
    }
    public void AddText()
    {
        while (index <= timer * speed)
        {
            sb.Append(endValue[index]);
            index++;
            onAddText?.Invoke();
        }

        setter(sb.ToString());
    }
    public override void Clear()
    {
        base.Clear();
        onAddText = null;
    }
}
public class ColorAlphaTweener : TweenerCore<Color,float,float>
{
    float speed;
    bool isPositive = false;
    public static ColorAlphaTweener Create()
    {
        return ReferencePool.Instance.Acquire<ColorAlphaTweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            Color cur = getter();
            Color color = new Color(cur.r, cur.g, cur.b, startValue);
            setter(color);
        }
        else
        {
            startValue = getter().a;
        }
        float dif = endValue - startValue;
        isPositive = dif >= 0 ? true : false;
        speed = Mathf.Abs(dif) / duration;
    }
    protected override void Do()
    {
        DoFade();
    }
    protected override void End()
    {
        Color cur = getter();
        Color color = new Color(cur.r, cur.g, cur.b, endValue);
        setter(color);
    }

    private void DoFade()
    {
        Color cur = getter();

        Color color = new Color(cur.r, cur.g, cur.b, cur.a);

        if (ScaleType == E_TimeScaleType.Normal)
        {
            if (isPositive)
            {
                color.a += speed * Time.deltaTime;
            }
            else
            {
                color.a -= speed * Time.deltaTime;
            }
        }
        else
        {
            if (isPositive)
            {
                color.a += speed * Time.unscaledDeltaTime;
            }
            else
            {
                color.a -= speed * Time.unscaledDeltaTime;
            }
        }

        setter(color);
    }
}

public class Vector2Tweener:TweenerCore<Vector2,Vector2,Vector2>
{
    Vector2 speed;
    public static Vector2Tweener Create()
    {
        return ReferencePool.Instance.Acquire<Vector2Tweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            setter(startValue);
        }
        else
        {
            startValue = getter();
        }
        speed.x = (endValue.x - startValue.x) / duration;
        speed.y = (endValue.y - startValue.y) / duration;
    }
    protected override void Do()
    {
        setter(startValue + timer * speed);
    }
    protected override void End()
    {
        setter(endValue);
    }
}

public class Vector3Tweener : TweenerCore<Vector3, Vector3, Vector3>
{
    Vector3 speed;
    public static Vector3Tweener Create()
    {
        return ReferencePool.Instance.Acquire<Vector3Tweener>();
    }
    public override void Restart()
    {
        base.Restart();
        if (isSetStartValue)
        {
            setter(startValue);
        }
        else
        {
            startValue = getter();
        }
        speed.x = (endValue.x - startValue.x) / duration;
        speed.y = (endValue.y - startValue.y) / duration;
        speed.z = (endValue.z - startValue.z) / duration;
    }
    protected override void Do()
    {
        setter(startValue + timer * speed);
    }
    protected override void End()
    {
        setter(endValue);
    }
}


