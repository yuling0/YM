using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class CharacterController2D : MonoBehaviour,IInit
{
    protected IFSM fsm;
    protected AnimationController ac;
    protected MovementController mc;
    public abstract void Init();


    public virtual void PlayAnim(string animName)
    {
        ac.PlayAnim(animName);
    }

    public virtual AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
    {
        return ac.GetCurrentAnimatorStateInfo(layerIndex);
    }
}
