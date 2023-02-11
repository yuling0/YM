using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Extension
{
    public static Transform FindChildTF(this Transform tf, string childName)
    {
        if (tf.name == childName)
        {
            return tf;
        }
        foreach (Transform child in tf)
        {
            Transform childTf = child.FindChildTF(childName);
            if (childTf != null) return childTf;
        }
        return null;
    }

    public static T GetControl<T>(this Transform transform, string controlName) where T : UIBehaviour
    {
        T[] cs = transform.GetComponentsInChildren<T>();
        foreach (var control in cs)
        {
            if (string.Equals(control.name, controlName, System.StringComparison.Ordinal))
            {
                return control;
            }
        }
        Debug.LogError($"{transform.name}中未找到名字为{controlName}的组件");
        return null;
    }

    public static T GetOrAddComponent<T>(this GameObject obj) where T : Component
    {
        T component = obj.GetComponent<T>();
        if (component == null)
        {
            component = obj.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// 判断一个整数是否解决一个小数（因为浮点类型有精度误差）
    /// </summary>
    /// <param name="num"></param>
    /// <param name="f"></param>
    /// <returns></returns>
    public static bool IsApproach(this int num, float f)
    {
        float precision = 0.01f;
        if (Mathf.Abs(num - f) <= precision) return true;
        return false;
    }

    public static void OnClickListener(this Button btn, UnityAction callback)
    {
        btn.onClick.RemoveListener(callback);
        btn.onClick.AddListener(callback);
    }

    public static T Parse<T>(string value) where T : Enum
    {
        return (T)Enum.Parse(typeof(T), value);
    }
    public static void Swap<T>(this Array arr, int idx1, int idx2)
    {
        T[] tempArray = arr as T[];
        if (idx1 != idx2)
        {
            T temp = tempArray[idx1];
            tempArray[idx1] = tempArray[idx2];
            tempArray[idx2] = temp;
        }
    }

    public static void Swap<T>(this List<T> arr, int idx1, int idx2)
    {
        if (idx1 != idx2)
        {
            T temp = arr[idx1];
            arr[idx1] = arr[idx2];
            arr[idx2] = temp;
        }
    }

    public static T SetAutoKill<T>(this T tweener, bool val) where T : Tween
    {
        if (!val)
        {
            TweenManager.Instance.CacheTweener(tweener);
        }
        tweener.IsAutoKill = val;
        return tweener;
    }
    public static T SetTarget<T>(this T tweener, object target) where T : Tween
    {
        tweener.target = target;
        return tweener;
    }
    public static AddTextTweener AddText(this Text text, string content, float duration)
    {
        AddTextTweener tweener = AddTextTweener.Create();
        tweener.setter = (txt) => text.text = txt;
        tweener.getter = () => text.text;
        tweener.SetParameter(content, duration);
        TweenManager.Instance.AddTweener(tweener);
        tweener.Restart();
        tweener.SetTarget(text);
        return tweener;
    }
    public static AddTextTweener AddText(this Text text, string startValue, string endValue, float duration)
    {
        AddTextTweener tweener = AddTextTweener.Create();
        tweener.setter = (txt) => text.text = txt;
        tweener.getter = () => text.text;
        tweener.SetParameter(startValue, endValue, duration);
        TweenManager.Instance.AddTweener(tweener);
        tweener.Restart();
        tweener.SetTarget(text);
        return tweener;
    }
    public static Tween DoText(this Text text, string content, float duration)
    {
        Tween t = TweenManager.To(() => text.text, (txt) => text.text = txt, content, duration);
        t.SetTarget(text);
        return t;
    }
    public static Tween DoText(this Text text, string startValue, string endValue, float duration)
    {
        Tween t = TweenManager.To(() => text.text, (txt) => text.text = txt, startValue, endValue, duration);
        t.SetTarget(text);
        return t;
    }

    public static Tween DoFade(this Image img, float startValue, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => img.color, (color) => img.color = color, startValue, endValue, duration);
        t.SetTarget(img);
        return t;
    }
    public static Tween DoFade(this Image img, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => img.color, (color) => img.color = color, endValue, duration);
        t.SetTarget(img);
        return t;
    }
    public static Tween DoScale(this Transform tf, Vector3 endValue, float duration)
    {
        Tween t = TweenManager.To(() => tf.localScale, (scale) => tf.localScale = scale, endValue, duration);
        t.SetTarget(tf);
        return t;
    }
    public static Tween DoScale(this Transform tf, Vector3 startValue, Vector3 endValue, float duration)
    {
        Tween t = TweenManager.To(() => tf.localScale, (scale) => tf.localScale = scale, startValue, endValue, duration);
        t.SetTarget(tf);
        return t;
    }

    public static Tween DoScaleX(this Transform tf, float endValue, float duration)
    {
        return tf.DoScale(new Vector3(endValue, tf.localScale.y, tf.localScale.z), duration);
    }

    public static Tween DoScaleY(this Transform tf, float endValue, float duration)
    {
        return tf.DoScale(new Vector3(tf.localScale.x, endValue, tf.localScale.z), duration);
    }
    public static Tween DoScaleZ(this Transform tf, float endValue, float duration)
    {
        return tf.DoScale(new Vector3(tf.localScale.x, tf.localScale.y, endValue), duration);
    }
    public static Tween DoAnchorPos(this RectTransform rt, Vector2 startValue, Vector2 endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, startValue, endValue, duration);
        t.SetTarget(rt);
        return t;
    }
    public static Tween DoAnchorPos(this RectTransform rt, Vector2 endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, endValue, duration);
        t.SetTarget(rt);
        return t;
    }
    public static Tween DoAnchorPosX(this RectTransform rt, Vector2 startValue, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, startValue, new Vector2(endValue, startValue.y), duration);
        t.SetTarget(rt);
        return t;
    }
    public static Tween DoAnchorPosX(this RectTransform rt, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, new Vector2(endValue, rt.anchoredPosition.y), duration);
        t.SetTarget(rt);
        return t;
    }

    public static Tween DoAnchorPosY(this RectTransform rt, Vector2 startValue, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, startValue, new Vector2(rt.anchoredPosition.x, endValue), duration);
        t.SetTarget(rt);
        return t;
    }
    public static Tween DoAnchorPosY(this RectTransform rt, float endValue, float duration)
    {
        Tween t = TweenManager.To(() => rt.anchoredPosition, (pos) => rt.anchoredPosition = pos, new Vector2(rt.anchoredPosition.x, endValue), duration);
        t.SetTarget(rt);
        return t;
    }

    public static void SetAlpha(this Image img, float val)
    {
        Color color = img.color;
        color.a = val;
        img.color = color;
    }
}
