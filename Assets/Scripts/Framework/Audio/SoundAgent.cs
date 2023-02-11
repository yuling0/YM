using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAgent : MonoBehaviour
{
    private AudioSource source;
    private Transform soundGroupTF;
    private int serialId;
    private int priority;
    private float volume;       //音量
    private float pitch;        //音高
    private bool loop;
    private float spatialBlend; //空间混合（设置2d音效和3d音效的权重：0 ~ 1）
    private float minDistance;  
    private float maxDistance;
    private AudioRolloffMode audioRolloffMode;  //3d音效随距离的衰减模式
    public void Init(Transform soundGroupTF)
    {
        source = gameObject.GetOrAddComponent<AudioSource>();
        this.soundGroupTF = soundGroupTF;
        serialId = -1;
    }
    public bool isPlayering => source.isPlaying;

    public int SerialId
    {
        get => serialId;
        set => serialId = value;
    }
    public int Priority 
    {
        get => priority;
        set
        {
            source.priority = value;
            priority = value;
        }
    }
    public float Volume 
    {
        get => volume;
        set
        {
            source.volume = value;
            volume = value;
        }
    }
    public float Pitch
    {
        get => pitch;
        set
        {
            source.pitch = value;
            pitch = value;
        }
    }
    public bool Loop 
    {
        get => loop;
        set
        {
            source.loop = value;
            loop = value;
        }
    }

    public float SpatialBlend 
    { 
        get => spatialBlend;
        set 
        {
            source.spatialBlend = value;
            spatialBlend = value;
        } 
    }
    public float MinDistance 
    { 
        get => minDistance;
        set 
        {
            source.minDistance = value;
            minDistance = value;
        } 
    }
    public float MaxDistance
    { 
        get => maxDistance;
        set
        {
            source.maxDistance = value;
            maxDistance = value;
        }
    }
    public AudioRolloffMode AudioRolloffMode 
    { 
        get => audioRolloffMode;
        set
        {
            source.rolloffMode = value;
            audioRolloffMode = value;
        }
    }
        
    public void SetVolume(float val)
        => source.volume = val;
    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }


    public void PlaySound(AudioClip clip,Transform followObj)
    {
        transform.SetParent(followObj);
        transform.localPosition = Vector3.zero;
        PlaySound(clip);
    }

    public void StopSound()
    {
        source.Stop();
    }

    public void PauseSound()
    {
        source.Pause();
    }
    public void ResumeSound()
    {
        source.Play();
    }
    public void Recycle()
    {
        transform.SetParent(soundGroupTF);
    }

}
