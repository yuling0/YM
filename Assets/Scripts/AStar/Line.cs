using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public struct Line
    {
        const float verticalLineGradient = 1e5f;

        float gradient;         //斜率
        float y_intercept;      //y截距
        float gradientPerpendicular;

        Vector2 pointOnLine_1;
        Vector2 pointOnLine_2;

        bool approachSide;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointOnLine">在转弯边界线上的一点</param>
        /// <param name="pointPerpendicularToLine">上一个转弯点</param>
        public Line(Vector2 pointOnLine, Vector2 pointPerpendicularToLine)
        {
            float dx = pointOnLine.x - pointPerpendicularToLine.x;
            float dy = pointOnLine.y - pointPerpendicularToLine.y;

            if (dx == 0)
            {
                gradientPerpendicular = verticalLineGradient;
            }
            else
            {
                gradientPerpendicular = dy / dx;    //这两点的斜率
            }

            if (gradientPerpendicular == 0)
            {
                gradient = verticalLineGradient;
            }
            else
            {
                gradient = -1 / gradientPerpendicular;      //转弯边界线的斜率 ：垂直与这两点的斜率
            }

            y_intercept = pointOnLine.y - gradient * pointOnLine.x;

            pointOnLine_1 = pointOnLine;    //边界线的上中点
            pointOnLine_2 = pointOnLine + new Vector2(1, gradient);  //边界上另一点

            approachSide = false;
            approachSide = GetSide(pointPerpendicularToLine);
        }


        bool GetSide(Vector2 p)     //二维向量叉乘求方位 ： 向量 (p2 - p1) 叉乘  向量 （p - p1）
        {
            return (p.x - pointOnLine_1.x) * (pointOnLine_2.y - pointOnLine_1.y) > (p.y - pointOnLine_1.y) * (pointOnLine_2.x - pointOnLine_1.x);
        }

        /// <summary>
        /// 是否经过边界线,经过了就要开始转弯
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public bool HasCrossedLine(Vector2 p)
        {
            return GetSide(p) != approachSide;
        }


        public void DrawWithGizmos(float len)
        {
            Vector3 lineDir = new Vector3(1, 0, gradient).normalized;
            Vector3 lineCentre = new Vector3(pointOnLine_1.x, 0, pointOnLine_1.y) + Vector3.up;
            Gizmos.DrawLine(lineCentre - lineDir * len / 2f, lineCentre + lineDir * len / 2);
        }
    }

}
