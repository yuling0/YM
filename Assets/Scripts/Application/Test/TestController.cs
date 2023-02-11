using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TestController : MonoBehaviour 
{
    private PlayableDirector director;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        TimelineAsset timelineAsset = director.playableAsset as TimelineAsset;

        foreach (var item in timelineAsset.outputs)
        {
            if (item.outputTargetType != null)
            {
                print(item.outputTargetType.Name);
                print(item.streamName);
            }
            
        }
    }
    private void Update()
    {
        if (InputMgr.Instance.GetKeyDown(Consts.K_Down))
        {
            director.time = 5;
            director.Stop();
        }

        print(director.state);
    }
}
