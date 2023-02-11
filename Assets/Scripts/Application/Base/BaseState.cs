using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseState : IState
{

    private Dictionary<Func<bool>, string> targetStateDic = new Dictionary<Func<bool>, string>();
    protected BaseFSM _fsm;
    protected Core _core;
    protected SoundHandler sh;
    protected List<string> characterSoundList;
    protected List<string> otherList;

    protected CharacterSoundData soundData;
    public BaseState(BaseFSM fsm,Core core)
    {
        this._fsm = fsm;
        this._core = core;
        sh = _core.GetComponentInCore<SoundHandler>();

        soundData = BinaryDataManager.Instance.GetContainer<CharacterSoundDataContainer>().GetCharacterSoundData(this.GetType().Name);
        characterSoundList = soundData?.characterSoundNames;

        otherList = soundData?.otherSoundNames;
    }

    public virtual void OnEnter()
    {
        PlayCharacterSound();
        if (soundData != null)
        {
            GlobalClock.Instance.AddTimer(soundData.delay, soundData.interval, soundData.time, PlayOtherSound);
        }
        
    }

    protected virtual void PlayCharacterSound()
    {
        if (characterSoundList == null) return;

        if (characterSoundList.Count > 0)
        {
            sh.PlayCharacterSound(characterSoundList[Random.Range(0, characterSoundList.Count)]);
        }
    }

    protected virtual void PlayOtherSound()
    {
        if (otherList == null) return;

        PlaySoundParams playSoundParams = PlaySoundParams.Create();
        playSoundParams.Volume = soundData.volume;
        playSoundParams.Pitch = soundData.pitch;
        if (otherList.Count > 0)
        {
            sh.PlaySound(otherList[Random.Range(0, otherList.Count)], playSoundParams);
        }
    }

    public virtual void OnConditionUpdate()
    {
        CheckTargetStateConditions();
    }


    public virtual void OnUpdate()
    {
        
    }

    public virtual void OnFixedUpdate()
    {
        
    }

    public virtual void AnimationEventTrigger()
    {


    }
    public virtual void OnExit()
    {
        GlobalClock.Instance.RemoveTimer(PlayOtherSound);
    }

    protected void AddTargetState(Func<bool> condition,string stateName)
    {
        if(!targetStateDic.ContainsKey(condition))
        {
            targetStateDic[condition] = stateName;
        }
    }

    protected void CheckTargetStateConditions()
    {
        foreach (var condition in targetStateDic.Keys)
        {
            if(condition.Invoke())
            {
                _fsm.ChangeState(targetStateDic[condition]);

                return;//这里巨重要,满足一个切换条件，后面切换条件就不执行了，不return可能会切换状态多次
            }
        }
    }


}
