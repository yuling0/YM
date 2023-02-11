using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : SingletonBase<CutsceneManager>
{
    private PlayableDirector playableDirector;
    private bool isPlaying;
    private CutsceneManager()
    {
        GameObject obj = new GameObject(nameof(CutsceneManager));
        playableDirector = obj.AddComponent<PlayableDirector>();
        Object.DontDestroyOnLoad(obj);
        isPlaying = false;
        playableDirector.playOnAwake = false;
        playableDirector.stopped += OnCutsceneStopped;
        playableDirector.paused += (pd) => { Debug.Log("暂停"); };
    }

    public bool PlayCutscene(TimelineAsset timelineAsset)
    {
        if (isPlaying)
        {
            throw new System.Exception("当前存在未播放玩的Timeline");
        }

        isPlaying = true;
        playableDirector.playableAsset = timelineAsset;
        playableDirector.Play(timelineAsset);
        return true;
    }
    public void Stop() 
    {
        if (!isPlaying) return;
        playableDirector.Stop();
    }

    public void Pause()
    {
        if (!isPlaying) return;
        playableDirector.Pause();
    }
    public void Resume()
    {
        if (!isPlaying) return;
        playableDirector.Resume();
    }

    private void OnCutsceneStopped(PlayableDirector playableDirector)
    {
        isPlaying = false;
        Debug.Log("播放完成");
    }
}
