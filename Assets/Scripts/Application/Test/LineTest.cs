using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineTest : MonoBehaviour
{
    LineRenderer line;
    public float r;
    public int pointCount;
    public float angle;
    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    private void OnEnable()
    {
        line.positionCount = pointCount;

        float angleBetween = angle / pointCount;


        for (int i = 0; i < pointCount; i++)
        {
            Vector3 pos = Quaternion.Euler(0, 0, i * -angleBetween) * -Vector3.right * r;
            line.SetPosition(i, pos);
            for (int j = i + 1; j < pointCount; j++)
            {
                line.SetPosition(j, pos);
            }

        }
    }
}
