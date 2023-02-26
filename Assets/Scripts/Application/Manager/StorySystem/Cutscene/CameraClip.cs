using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraClip : PlayableAsset
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    private CameraBehaviour template = new CameraBehaviour();
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<CameraBehaviour> playable = ScriptPlayable<CameraBehaviour>.Create(graph, template);
        CameraBehaviour clone = playable.GetBehaviour();
        clone.startPosition = startPosition;
        clone.endPosition = endPosition;
        return playable;
    }
}
