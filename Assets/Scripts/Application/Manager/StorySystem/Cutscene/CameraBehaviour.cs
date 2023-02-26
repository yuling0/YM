using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraBehaviour : PlayableBehaviour
{
    public Vector3 startPosition;
    public Vector3 endPosition;
    private Vector3 velocity;
    private bool isStart;
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.Log(playable.GetDuration());
        velocity = (endPosition - startPosition) / (float)playable.GetDuration();
        CameraController.Instance.transform.position = startPosition;
        isStart = true;
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        CameraController.Instance.isFollow = false;
        CameraController.Instance.transform.position += velocity * Time.deltaTime;
    }

    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        if (isStart)
        {
            CameraController.Instance.transform.position = endPosition;
        }
    }

    public override void OnGraphStart(Playable playable)
    {
        Debug.Log("OnGraphStart");
    }
    public override void OnGraphStop(Playable playable)
    {
        Debug.Log("OnGraphStop");
    }
}
