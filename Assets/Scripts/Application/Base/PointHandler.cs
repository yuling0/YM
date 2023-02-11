using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 巡逻点和出生点管理者
/// </summary>
public class PointHandler : ComponentBase
{
    private Transform bornPoint;
    private List<Transform> patrolPoints;
    private int curIndex;

    public Vector3 GetBornPoint => bornPoint.position;

    public Transform GetCurrentPatrolPoint
    {
        get
        {
            if (patrolPoints.Count == 0)
            {
                return null;
            }
            return patrolPoints[curIndex = (curIndex + 1) % patrolPoints.Count ];
        }
    }
    public override void Init(Core obj)
    {
        base.Init(obj);
        patrolPoints = new List<Transform>();
        bornPoint = transform.Find("BornPoint");
        foreach (var t in transform)
        {
            if (t as Transform != bornPoint) patrolPoints.Add(t as Transform);
        }
    }


}
