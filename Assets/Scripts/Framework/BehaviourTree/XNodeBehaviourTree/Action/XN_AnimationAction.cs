using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    public class XN_AnimationAction : XN_Task
    {
        AnimationController ac;
        [LabelText("动画名称")]
        public string animationName;

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

            Debug.Log($"播放了动画{animationName}");
            Root.Clock.AddTimer(0.02f, 0, WaitAnimationComplete);
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
                Root.Clock.AddTimer(0, 0, WaitAnimationComplete);
            }
        }

        protected override void DoStop()
        {

            Stopped(false);
        }

        public override NodeDataBase CreateNodeData(int id)
        {
            return new AnimationActionData() {id = id, animationName = animationName, NodeType = NodeDataBase.E_NodeType.Task };
        }

        public override Node CreateNode()
        {
            AnimationAction action = ReferencePool.Instance.Acquire<AnimationAction>();
            action.animationName = animationName;
            return action;
        }
    }

}
