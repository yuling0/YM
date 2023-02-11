using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformTest : MonoBehaviour
{
    RectTransform rt;
    public Image img;
    // Start is called before the first frame update
    void Start()
    {
        rt = transform as RectTransform;
    }

    // Update is called once per frame
    void Update()
    {
        print(rt.rect.min);
        print(rt.rect.max);
        //print(rt.rect.center);
        print(rt.localPosition);
        print(rt.offsetMin);
        print(rt.offsetMax);
        print(rt.anchoredPosition);
    }

    Vector2 GetLocalPos(Rect rect, Vector2 pivot)
    {
        Vector2 localPosition2D = rt.localPosition;
        return rect.min + localPosition2D + Vector2.Scale(rect.size, pivot);
    }
}
