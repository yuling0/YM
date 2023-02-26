using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    private DialogueBehaviour template = new DialogueBehaviour();
    public string dialogDataName;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueBehaviour>.Create(graph,template);
        DialogueBehaviour clone = playable.GetBehaviour();
        clone.dialogDataName = dialogDataName;
        return playable;
    }
}
