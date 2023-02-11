using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class AStarPath
    {
        public readonly Vector3[] lookPoints;
        public readonly Line[] turnBoundaries;
        public readonly int finishLineIndex;

        public AStarPath(Vector3[] wayPoint, Vector3 startPos, float turnDst)
        {
            lookPoints = wayPoint;      //路径点

            turnBoundaries = new Line[lookPoints.Length];  //转弯的边界线

            finishLineIndex = turnBoundaries.Length - 1;    //目标点的索引

            Vector2 previousPoint = V3ToV2(startPos);

            for (int i = 0; i < lookPoints.Length; i++)
            {
                Vector2 currentPoint = V3ToV2(lookPoints[i]);
                Vector2 dirToCurrentPoint = (currentPoint - previousPoint).normalized;
                Vector2 turnBoundaryPoint = (i == finishLineIndex) ? currentPoint : currentPoint - dirToCurrentPoint * turnDst; //该line上的转弯点
                turnBoundaries[i] = new Line(turnBoundaryPoint, previousPoint - dirToCurrentPoint * turnDst);

                previousPoint = turnBoundaryPoint;
            }
        }

        Vector2 V3ToV2(Vector3 v3)
        {
            return new Vector2(v3.x, v3.z);
        }

        public void DrawWithGizmos()
        {
            Gizmos.color = Color.black;
            foreach (Vector3 p in lookPoints)
            {
                Gizmos.DrawCube(p + Vector3.up, Vector3.one);
            }

            Gizmos.color = Color.white;
            foreach (Line l in turnBoundaries)
            {
                l.DrawWithGizmos(10);
            }
        }
    }

}
