using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : SingletonBase<CutsceneManager>
{
    private PlayableDirector playableDirector;
    private bool isPlaying;
    private StorySystem storySystem;
    private Dictionary<string, TimelineAsset> cutSceneDic;

    public bool IsPlaying => isPlaying;
    private CutsceneManager()
    {
        storySystem = StorySystem.Instance;
        cutSceneDic = new Dictionary<string, TimelineAsset>();
        GameObject obj = new GameObject(nameof(CutsceneManager));
        playableDirector = obj.AddComponent<PlayableDirector>();
        Object.DontDestroyOnLoad(obj);
        
    }
    public void Init()
    {
        isPlaying = false;
        playableDirector.playOnAwake = false;
        playableDirector.stopped += OnCutsceneStopped;
        playableDirector.paused += (pd) => { Debug.Log("暂停"); };

        TimelineAsset[] allCutscene = ResourceMgr.Instance.LoadAllAsset<TimelineAsset>(Consts.P_CutscenePath);
        foreach (var cutscene in allCutscene)
        {
            cutSceneDic.Add(cutscene.name, cutscene);
        }
        EventMgr.Instance.AddEventListener<StoryEventArgs>(Consts.E_PlayCutscene, PlayCutscene);
    }
    public void PlayCutscene(StoryEventArgs storyEventArgs)
    {
        if (cutSceneDic.TryGetValue(storyEventArgs.stringArgs, out TimelineAsset timelineAsset))
        {
            PlayCutscene(timelineAsset);
        } 
    }
    public void PlayCutscene(TimelineAsset timelineAsset)
    {
        if (isPlaying)
        {
            throw new System.Exception("当前存在未播放玩的Timeline");
        }
        //UIManager.Instance.Push<StoryPanel>();
        isPlaying = true;
        playableDirector.playableAsset = timelineAsset;
        playableDirector.Play(timelineAsset);
        GameManager.DisablePlayerControl();
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
        if (!storySystem.CheckEventTriggerOnCutscenePlayed(playableDirector.playableAsset.name))
        {
            GameManager.EnablePlayerControl();
        }
        Debug.Log("播放完成");
    }
}
