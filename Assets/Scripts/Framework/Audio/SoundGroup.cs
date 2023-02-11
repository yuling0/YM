using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundGroup : MonoBehaviour
{
    private List<SoundAgent> soundAgents;
    private int maxCount;
    private string groupName;
    public void Init(E_SoundType soundType,Transform soundManagerTF ,int soundAgentCount = 16)
    {
        soundAgents = new List<SoundAgent>();
        maxCount = soundAgentCount;
        groupName = soundType.ToString();
        gameObject.name = groupName;
        transform.SetParent(soundManagerTF);

        for (int i = 0; i < maxCount; i++)
        {
            var obj = ResourceMgr.Instance.LoadAsset<GameObject>(Consts.P_SoundAgent);
            var agent = obj.GetOrAddComponent<SoundAgent>();
            soundAgents.Add(agent);
            agent.Init(transform);
            agent.Recycle();
        }
    }

    public void SetVolume(float volume)
    {
        foreach (var agent in soundAgents)
        {
            agent.SetVolume(volume);
        }
    }

    private SoundAgent GetSoundAgent(int priority)
    {
        SoundAgent target = null;
        foreach (var agent in soundAgents)
        {
            if (!agent.isPlayering)
            {
                target = agent;
                break;
            }

            if (agent.Priority < priority)
            {
                target = agent;
            }
        }
        return target;
    }
    public void PlaySound(int serialId, AudioClip clip, PlaySoundParams playSoundParams)
    {
        SoundAgent target = GetSoundAgent(playSoundParams.Priority);
        if(target == null)
        {
            Debug.Log("当前播放队列已满,无法播放名称为：" + clip.name + "的音效切片文件");
            return;
        }
        target.SerialId = serialId;
        target.Priority = playSoundParams.Priority;
        target.Volume = playSoundParams.Volume;
        target.Pitch = playSoundParams.Pitch;
        target.Loop = playSoundParams.Loop;
        target.SpatialBlend = playSoundParams.SpatialBlend;
        target.MinDistance = playSoundParams.MinDistance;
        target.MaxDistance = playSoundParams.MaxDistance;
        target.AudioRolloffMode = playSoundParams.AudioRolloffMode;

        ReferencePool.Instance.Release(playSoundParams);
        target.PlaySound(clip);
    }


    public void PlaySound(int serialId , AudioClip clip, Transform followObj, PlaySoundParams playSoundParams)
    {
        SoundAgent target = GetSoundAgent(playSoundParams.Priority);
        if (target == null)
        {
            Debug.Log("当前播放队列已满,无法播放名称为：" + clip.name + "的音效切片文件");
            return;
        }
        target.SerialId = serialId;
        target.Priority = playSoundParams.Priority;
        target.Volume = playSoundParams.Volume;
        target.Pitch = playSoundParams.Pitch;
        target.Loop = playSoundParams.Loop;
        target.SpatialBlend = playSoundParams.SpatialBlend;
        target.MinDistance = playSoundParams.MinDistance;
        target.MaxDistance = playSoundParams.MaxDistance;
        target.AudioRolloffMode = playSoundParams.AudioRolloffMode;

        ReferencePool.Instance.Release(playSoundParams);
        target.PlaySound(clip, followObj);
    }

    public bool StopSound(int serialId)
    {
        foreach (var agent in soundAgents)
        {
            if (agent.SerialId == serialId)
            {
                agent.StopSound();
                return true;
            }
        }
        return false;
    }

    public bool PauseSound(int serialId)
    {
        foreach (var agent in soundAgents)
        {
            if (agent.SerialId == serialId)
            {
                agent.PauseSound();
                return true;
            }
        }
        return false;
    }

    public bool ResumeSound(int serialId)
    {
        foreach (var agent in soundAgents)
        {
            if (agent.SerialId == serialId)
            {
                agent.ResumeSound();
                return true;
            }
        }
        return false;
    }
}
