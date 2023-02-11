using NPBehave;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static NPBehave.Action;

public class BTTest : MonoBehaviour
{

    public Vector2 v1;
    public Vector2 v2;

    private void Start()
    {
        //int i = 0;
        //Root bt = new Root(
        //    new Parallel(Parallel.Policy.ALL, Parallel.Policy.ONE, new Action(() => { Debug.Log("oyy"); }),
        //    new Action((bool oyy) => 
        //    {
        //        Debug.Log(i);
        //        if (oyy)
        //        {
        //            return Result.FAILED;
        //        }
        //        else
        //        {
        //            i++;

        //            if(i>5)
        //            {
        //                return Result.SUCCESS;
        //            }
        //            return Result.PROGRESS;
        //        }
        //    }),
        //    new Action(() => { Debug.Log("xld？"); })
        //    )
        //);

        //bt.Start();

    }

    private void Update()
    {
        //print(Vector2.Angle(v1, v2));

        print(Mathf.Atan2(v2.y, v2.x) / Mathf.PI * 180);
        print(Atan2(v2.y, v2.x) / Mathf.PI * 180);

        Debug.DrawLine(Vector2.zero, v1);
        Debug.DrawLine(Vector2.zero, v2);
    }

    //分段函数思想
    private float Atan2(float y , float x)
    {
        if (x == 0 && y == 0) return 0f;
        if(x > 0)
        {
            return Mathf.Atan(y / x);
        }

        else if(x == 0)
        {
            return y > 0 ? Mathf.PI / 2 : -Mathf.PI / 2;
        }

        else
        {
            if (y >= 0)
            {
                return Mathf.Atan(-x / y) + Mathf.PI / 2;
            }
            else
            {
                return -Mathf.Atan( x / y ) - Mathf.PI / 2;
            }
        }
    }
}

