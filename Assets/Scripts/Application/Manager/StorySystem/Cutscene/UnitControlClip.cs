using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UnitControlClip : PlayableAsset
{
    public int unitId;
    [EnumPaging]
    public E_ControlType controlType;
    public float moveOffset;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<UnitControlBehaviour> playable = ScriptPlayable<UnitControlBehaviour>.Create(graph);
        UnitControlBehaviour clone = playable.GetBehaviour();
        clone.unitId = unitId;
        clone.controlType = controlType;
        clone.moveOffset = moveOffset;
        return playable;
    }
}
