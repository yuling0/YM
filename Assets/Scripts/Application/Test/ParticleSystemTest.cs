using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemTest : MonoBehaviour
{
    public ParticleSystem ps;
    public float intervalTime;
    public Vector2Int emitCount;
    public float nextEmitTime;
    public int Clycle;
    public int curClycle;
    public bool isAllowGenerate;
    public float startTime;
    public float emitDurationTime;
    public float endTime;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            isAllowGenerate = !isAllowGenerate;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            isAllowGenerate = true;
            curClycle = 0;
            startTime = Time.time;
            endTime = startTime + emitDurationTime;
        }

        //if (curClycle < Clycle)
        //{
        //    if (Time.time > nextEmitTime)
        //    {
        //        if (isAllowGenerate)
        //        {
        //            ps.Emit(Random.Range(emitCount.x,emitCount.y + 1));
        //        }
        //        curClycle++;
        //        nextEmitTime = Time.time + intervalTime;
        //    }
        //}
        if (Time.time < endTime)
        {
            if (Time.time > nextEmitTime)
            {
                if (isAllowGenerate)
                {
                    ps.Emit(Random.Range(emitCount.x, emitCount.y + 1));
                }
                curClycle++;
                nextEmitTime = Time.time + intervalTime;
            }
        }

    }
}
