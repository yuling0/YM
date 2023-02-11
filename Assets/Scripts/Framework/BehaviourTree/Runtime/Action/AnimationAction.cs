using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class AnimationAction : Task
    {
        AnimationController ac;

        public string animationName;

        public AnimationAction(string animationName) : base("AnimationAction")
        {
            this.animationName = animationName;
        }

        public AnimationAction() : base("AnimationAction")
        {

        }

        public override void OnInit()
        {
            ac = GetComponentInCore<AnimationController>();
        }

        protected override void DoStart()
        {
            PlayAnimation();
        }
        private void PlayAnimation()
        {
            ac.PlayAnim(animationName);
            Stopped(true);
        }

        private void WaitAnimationComplete()
        {
            if (ac.CurAnimNormalizedTime >= 1)
            {
                Debug.Log($"动画{animationName}结束了");
                Stopped(true);
            }
            else
            {
                Debug.Log($"等待动画{animationName}结束");
                Root.Clock.AddTimer(0, 0, WaitAnimationComplete);
            }
        }

        protected override void DoStop()
        {
            Root.Clock.RemoveTimer(WaitAnimationComplete);
            Stopped(false);
        }
    }
}

