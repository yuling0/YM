using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Map : MonoBehaviour
{
    public Vector2 size;
    public Vector3 offset;
    public Vector4 range;

    void Update()
    {
        DrawUtility.DrawRectangle(transform.position + offset, size, Color.green);
        range = GetLimitRange();
    }

    public Vector4 GetLimitRange()
    {
        Vector4 vec = new Vector4();
        vec.x = transform.position.x + offset.x - size.x;       //左边界坐标
        vec.y = transform.position.x + offset.x + size.x;       //右边界坐标
        vec.w = transform.position.y + offset.y + size.y;       //上边界坐标
        vec.z = transform.position.y + offset.y - size.y;       //下边界坐标

        return vec;
    }
}
