using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO:音效管理需要在游戏开始时，初始化一下加载音效（偷懒不实现异步加载了，思路：SoundManager实现异步加载音效文件，SoundGroup异步加载AudioAgent）
public class SoundManager : SingletonBase<SoundManager>
{
    private AudioSource bgmSource;
    private GameObject soundManagerObj;
    private ResourceMgr resourceMgr;
    private Dictionary<string, AudioClip> clipDic;
    private Dictionary<E_SoundType, SoundGroup> groupDic;       //此处可能有装箱
    private int serial;
    private float mainVolume;
    private float bgmVolume;
    private float soundVolume;

    public float MainVolume
    {
        get => mainVolume;
        set
        {
            mainVolume = Mathf.Clamp01(value);
            BGMVlome = bgmVolume;                   //更新bgm音量
            SoundVolume = soundVolume;              //更新sound音量
        }
    }

    public float BGMVlome
    {
        get => bgmVolume;
        set
        {
            bgmVolume = mainVolume * Mathf.Clamp01(value);
            SetBGMVolume();
        }
    }

    public float SoundVolume
    {
        get => soundVolume;
        set
        {
            soundVolume = mainVolume * Mathf.Clamp01(value);
            SetSoundVolume();
        }
    }

    private SoundManager()
    {
        soundManagerObj = new GameObject() { name = "AudioMgr" };
        GameObject.DontDestroyOnLoad(soundManagerObj);
        bgmSource = soundManagerObj.GetOrAddComponent<AudioSource>();
        groupDic = new Dictionary<E_SoundType, SoundGroup>();
        clipDic = new Dictionary<string, AudioClip>();
        serial = 0;
        resourceMgr = ResourceMgr.Instance;
    }
    public void Init()
    {
        InitGroup();
        LoadAllClips();
    }

    private void InitGroup()
    {
        foreach (var soundType in Enum.GetValues(typeof(E_SoundType)))
        {
            E_SoundType type = (E_SoundType)soundType;
            var obj = resourceMgr.LoadAsset<GameObject>(Consts.P_SoundGroup);
            SoundGroup group = obj.GetOrAddComponent<SoundGroup>();
            groupDic.Add(type, group);
            group.Init(type, soundManagerObj.transform);
        }
    }



    private void LoadAllClips()
    {
        AudioClip[] clips = ResourceMgr.Instance.LoadAllAsset<AudioClip>(Consts.P_AudioClipPath);
        foreach (var c in clips)
        {
            clipDic.Add(c.name, c);
        }
    }

    private void SetBGMVolume()
        => bgmSource.volume = bgmVolume;
    private void SetSoundVolume()
    {
        foreach (var group in groupDic.Values)
        {
            group.SetVolume(soundVolume);
        }
    }
    public void PlayBGM(string clipName)
    {
        if (clipDic.TryGetValue(clipName, out AudioClip clip))
        {
            bgmSource.clip = clip;
            bgmSource.Play();
            return;
        }
        Debug.LogError("未找名称为" + clipName + "音效文件");
    }

    public int PlaySound(E_SoundType soundType, string clipName)
    {
        return PlaySound(soundType, clipName, PlaySoundParams.Create());
    }
    public int PlaySound(E_SoundType soundType, string clipName, PlaySoundParams playSoundParams)
    {
        int serialId = serial++;
        if (clipDic.TryGetValue(clipName, out AudioClip clip))
        {
            SoundGroup group = groupDic[soundType];
            group.PlaySound(serialId, clip, playSoundParams);
            return serialId;
        }
        Debug.LogError("未找名称为" + clipName + "音效文件");
        return -1;
    }

    public int PlaySound(E_SoundType soundType, string clipName, Transform followObj)
    {
        return PlaySound(soundType, clipName, followObj, PlaySoundParams.Create());
    }

    public int PlaySound(E_SoundType soundType, string clipName, Transform followObj, PlaySoundParams playSoundParams)
    {
        if (clipDic.TryGetValue(clipName, out AudioClip clip))
        {
            SoundGroup group = groupDic[soundType];
            int serialId = serial++;
            group.PlaySound(serialId, clip, followObj, playSoundParams);
            return serialId;
        }
        Debug.LogError("未找名称为" + clipName + "音效文件");
        return -1;
    }

    public bool StopSound(int serialId)
    {
        foreach (var soundGroup in groupDic.Values)
        {
            if (soundGroup.StopSound(serialId))
            {
                return true;
            }
        }
        return false;
    }
    public bool PauseSound(int serialId)
    {
        foreach (var soundGroup in groupDic.Values)
        {
            if (soundGroup.PauseSound(serialId))
            {
                return true;
            }
        }
        return false;
    }
    public bool ResumeSound(int serialId)
    {
        foreach (var soundGroup in groupDic.Values)
        {
            if (soundGroup.ResumeSound(serialId))
            {
                return true;
            }
        }
        return false;
    }
}
