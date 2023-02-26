using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateMonsterInfo : IReference
{
    public List<Vector3> patrolPoints;
    public Vector3 birthPoint;

    public static CreateMonsterInfo Create(List<Vector3> patrolPoints, Vector3 birthPoint)
    {
        CreateMonsterInfo createMonsterInfo = ReferencePool.Instance.Acquire<CreateMonsterInfo>();
        createMonsterInfo.patrolPoints= patrolPoints;
        createMonsterInfo.birthPoint= birthPoint;
        return createMonsterInfo;
    }
    public void Clear()
    {
        patrolPoints = null;
        birthPoint = Vector3.zero;
    }
}
