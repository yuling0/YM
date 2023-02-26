using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public enum E_ControlType
{
    walk,
    run,
    flip
}
public class UnitControlBehaviour : PlayableBehaviour
{
    public int unitId;
    public E_ControlType controlType;
    public float moveOffset;
    public bool isPlayed;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (!isPlayed)
        {
            CutsceneManager.Instance.Pause();
            switch (controlType)
            {
                case E_ControlType.walk:
                    UnitManager.Instance.UnitMoveTowards(unitId, moveOffset, () => { CutsceneManager.Instance.Resume(); });
                    break;

                case E_ControlType.run:
                    UnitManager.Instance.UnitRunTowards(unitId, moveOffset, () => { CutsceneManager.Instance.Resume(); });
                    break;
                case E_ControlType.flip:
                    UnitManager.Instance.UnitFlip(unitId, () => { CutsceneManager.Instance.Resume(); });
                    break;
            }
            isPlayed = true;
        }

    }
}
