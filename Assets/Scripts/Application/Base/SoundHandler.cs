using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHandler : ComponentBase
{
    private SoundManager sm;
    public float characterSoundIntervalTimer;       //播放角色音效时间间隔
    public float intervalTimer;

    public override void Init(Core core)
    {
        base.Init(core);
        sm = SoundManager.Instance;
    }

    public void PlayCharacterSound(string clipName)
    {
        if (intervalTimer > 0f) return;

        sm.PlaySound(E_SoundType.Character, clipName, transform);
        intervalTimer = characterSoundIntervalTimer;
    }



    public void PlaySound(string clipName,PlaySoundParams playSoundParams)
    {
        sm.PlaySound(E_SoundType.Other, clipName, transform, playSoundParams);
    }

    public override void OnUpdateComponent()
    {
        if (intervalTimer <= 0) return;

        intervalTimer -= Time.deltaTime;
    }
}
