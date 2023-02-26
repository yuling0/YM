using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueBehaviour : PlayableBehaviour
{
    public string dialogDataName;
    public bool isPlayed;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!isPlayed)
        {
            CutsceneManager.Instance.Pause();
            DialogueArgs args = DialogueArgs.Create(dialogDataName, () => { CutsceneManager.Instance.Resume(); });
            DialogueManager.Instance.StartDialog(args);
            Debug.Log($"该Playable的持续时间：{playable.GetDuration()}");
            isPlayed = true;
        }

    }

    //public override void OnBehaviourPause(Playable playable, FrameData info)
    //{
    //    if (isPlaying)
    //    {
    //        //CutsceneManager.Instance.Pause();
    //        isPlaying = false;
    //    }
    //}
}
