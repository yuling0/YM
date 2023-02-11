using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogBehaviour : PlayableBehaviour
{
    public DialogDataContainer dialogData;
    public bool isPlaying;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        DialogManager.Instance.StartDialog(dialogData);
        Debug.Log($"��Playable�ĳ���ʱ�䣺{playable.GetDuration()}");
        isPlaying = true;

    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (isPlaying)
        {
            CutsceneManager.Instance.Pause();
            isPlaying = false;
        }
    }
}
