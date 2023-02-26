using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 巡逻点和出生点管理者
/// </summary>
public class PointHandler : ComponentBase
{
    private Vector3 birthPoint;
    private List<Vector3> patrolPoints;
    private int curIndex;

    public Vector3 GetBirthPoint => birthPoint;

    public Vector3 GetCurrentPatrolPoint
    {
        get
        {
            if (patrolPoints.Count == 0)
            {
                return birthPoint;
            }
            return patrolPoints[curIndex = (curIndex + 1) % patrolPoints.Count ];
        }
    }
    public override void Init(Core obj, object userData)
    {
        base.Init(obj,userData);
        //patrolPoints = new List<Transform>();
        //bornPoint = transform.Find("BornPoint");
        //foreach (var t in transform)
        //{
        //    if (t as Transform != bornPoint) patrolPoints.Add(t as Transform);
        //}
        CreateMonsterInfo  createMonsterInfo = userData as CreateMonsterInfo;
        if (createMonsterInfo != null)
        {
            patrolPoints = createMonsterInfo.patrolPoints;
            birthPoint = createMonsterInfo.birthPoint;
        }
    }


}
