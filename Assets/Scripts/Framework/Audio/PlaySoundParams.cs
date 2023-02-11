using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundParams : IReference
{
    int priority;
    float volume;       //音量
    float pitch;        //音高
    bool loop;
    float spatialBlend; //空间混合（设置2d音效和3d音效的权重：0 ~ 1）
    float minDistance;
    float maxDistance;
    AudioRolloffMode audioRolloffMode;  //3d音效随距离的衰减模式

    public int Priority { get => priority; set => priority = value; }
    public float Volume { get => volume; set => volume = value; }
    public float Pitch { get => pitch; set => pitch = value; }
    public bool Loop { get => loop; set => loop = value; }
    public float SpatialBlend { get => spatialBlend; set => spatialBlend = value; }
    public float MinDistance { get => minDistance; set => minDistance = value; }
    public float MaxDistance { get => maxDistance; set => maxDistance = value; }
    public AudioRolloffMode AudioRolloffMode { get => audioRolloffMode; set => audioRolloffMode = value; }

    public PlaySoundParams()
    {
        Init();
    }

    private void Init()
    {
        priority = 1;
        volume = 1;
        pitch = 1;
        loop = false;
        SpatialBlend = 0f;
        MinDistance = 2f;
        MaxDistance = 100f;
        AudioRolloffMode = AudioRolloffMode.Logarithmic;
    }
    public static PlaySoundParams Create()
    {
        return ReferencePool.Instance.Acquire<PlaySoundParams>();
    }
    public void Clear()
    {
        Init();
    }
}
