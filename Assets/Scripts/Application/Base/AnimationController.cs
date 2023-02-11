using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 动画控制器
/// </summary>
public class AnimationController : ComponentBase
{
    private Animator anim;
    private string curAnim;         //当前播放的动画名
    private SpriteRenderer _spriteRenderer;
    private SoundHandler soundHandler;
    public float CurAnimNormalizedTime => anim.GetCurrentAnimatorStateInfo(0).normalizedTime;  //获得当前动画的播放进度

    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if(_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    public override void Init(Core obj)
    {
        base.Init(obj);
        soundHandler = GetComponentInCore<SoundHandler>();
        anim = GetComponent<Animator>();
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="animName"></param>
    public void PlayAnim(string animName)
    {
        if (animName == curAnim)
        {
            return;
        }
        anim.Play(animName);
        curAnim = animName;
        
    }

    /// <summary>
    /// 获取当前动画信息
    /// </summary>
    /// <param name="layerIndex"></param>
    /// <returns></returns>
    public AnimatorStateInfo GetCurrentAnimatorStateInfo(int layerIndex)
    {
        return anim.GetCurrentAnimatorStateInfo(layerIndex);
    }

    public void SetLayerWeight(int layerIndex,float weight)
    {
        anim.SetLayerWeight(layerIndex, weight);
    }
}
