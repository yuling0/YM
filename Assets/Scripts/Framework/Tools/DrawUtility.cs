using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//绘制工具

public static class DrawUtility
{
    /// <summary>
    /// 绘制圆形范围
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="color">颜色</param>
    public static void DrawCircle(Transform tf , float r , Color color)
    {
        Vector2 origin = tf.position + tf.right * r;
        Vector2 pre = origin;
        for (int i = 0; i < 360; i++)
        {
            Vector2 cur = tf.position + (Quaternion.AngleAxis(i, tf.forward) * tf.right * r);
            Debug.DrawLine(pre, cur, color);
            pre = cur;
        }
        Debug.DrawLine(pre, origin, color);
    }

    /// <summary>
    /// 绘制扇形    
    /// </summary>
    /// <param name="r">半径</param>
    /// <param name="color">颜色</param>
    /// <param name="angle">角度</param>
    /// <param name="isFacingRight">是否面向右边</param>
    /// <param name="height">高度偏移</param>
    public static void DrawSector(Transform tf , float r , Color color , int angle , bool isFacingRight, float height = 0f)
    {
        Vector2 origin = (Vector2)tf.position + Vector2.up * height;

        Vector2 pre = origin + (Vector2)(Quaternion.AngleAxis(angle / 2, tf.forward) * (isFacingRight ? tf.right : -tf.right) * r);
        Debug.DrawLine(origin, pre, color);
        for (int i = angle / 2; i >= -angle / 2; i--)
        {
            Vector2 cur = origin + (Vector2)(Quaternion.AngleAxis(i, tf.forward) * (isFacingRight ? tf.right : -tf.right) * r);

            Debug.DrawLine(pre, cur, color);
            pre = cur;
        }
        Debug.DrawLine(origin, pre, color);
    }

    /// <summary>
    /// 绘制矩形
    /// </summary>
    /// <param name="tf"></param>
    /// <param name="size"></param>
    /// <param name="color"></param>
    public static void DrawRectangle(Transform tf , Vector2 size , Color color)
    {
        DrawRectangle(tf.position, size, color);
    }

    public static void DrawRectangle(Vector3 origin, Vector2 size, Color color)
    {
        Vector3 v1 = origin + new Vector3(-size.x, size.y);
        Vector3 v2 = origin + new Vector3(-size.x, -size.y);
        Vector3 v3 = origin + new Vector3(size.x, -size.y);
        Vector3 v4 = origin + new Vector3(size.x, size.y);
        Debug.DrawLine(v1, v2, color);
        Debug.DrawLine(v2, v3, color);
        Debug.DrawLine(v3, v4, color);
        Debug.DrawLine(v1, v4, color);
    }
}
