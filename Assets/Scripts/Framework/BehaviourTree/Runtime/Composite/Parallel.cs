using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace YMFramework.BehaviorTree
{
    public class Parallel : Composite
    {
        //public enum E_Policy
        //{
        //    One,
        //    ALL
        //}
        public E_Policy failurePolicy;     //失败时的策略
        public E_Policy successPolicy;     //成功时的策略
        int childCount;
        int runningCount;
        int sucessCount;
        int failureCount;

        Dictionary<Node, bool> childrenResults = new Dictionary<Node, bool>();
        bool childNodeAbort;

        public Parallel(params Node[] childs) : base("Parallel", childs)
        {

        }
        public Parallel(E_Policy successPolicy, E_Policy failurePolicy, params Node[] childs) : base("Parallel", childs)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
        }

        public Parallel():base("Parallel")
        {

        }

        protected override void DoStart()
        {
            base.DoStart();
            childCount = children.Count;
            runningCount = 0;
            sucessCount = 0;
            failureCount = 0;
            childNodeAbort = false;
            childrenResults.Clear();
            ProcessChildNode();
        }
        private void ProcessChildNode()
        {
            foreach (var childNode in children)
            {
                childNode.Start();
                runningCount++;
            }
        }

        protected override void DoStop()
        {
            foreach (var childNode in children)
            {
                if (childNode.IsActive)
                {
                    childNode.Stop();
                }
            }
        }

        protected override void DoChildStopped(Node child, bool result)
        {
            runningCount--;
            childrenResults[child] = result;
            if (result)
            {
                sucessCount++;
            }
            else
            {
                failureCount++;
            }
            bool allChildNodeStarted = runningCount + sucessCount + failureCount == childCount;

            if (allChildNodeStarted)
            {
                if (runningCount == 0)      //子节点已经全部运行完成
                {
                    if (successPolicy == E_Policy.One && sucessCount > 0)
                    {
                        Stopped(true);
                    }
                    else if (failurePolicy == E_Policy.One && failureCount > 0)
                    {
                        Stopped(false);
                    }
                    else if (successPolicy == E_Policy.ALL && sucessCount == childCount)
                    {
                        Stopped(true);
                    }
                    else
                    {
                        Stopped(false);
                    }
                }
                else        // 子节点还有在运行的
                {
                    if (successPolicy == E_Policy.One && sucessCount > 0)
                    {
                        childNodeAbort = true;
                    }
                    else if (failurePolicy == E_Policy.One && failureCount > 0)
                    {
                        childNodeAbort = true;
                    }

                    if (childNodeAbort)     //已经可以返回执行结果了，终止正在运行的子节点
                    {
                        foreach (var childNode in children)
                        {
                            if (childNode.IsActive)
                            {
                                childNode.Stop();
                            }
                        }
                    }

                }
            }
        }


        public override void StopLowerPriorityChildrenForChild(Node childNode, bool isimmediateRestart)
        {
            if (isimmediateRestart)
            {
                if(childrenResults[childNode])
                {
                    sucessCount--;
                }
                else
                {
                    failureCount--;
                }
                runningCount++;
                childNode.Start();
            }
            else
            {
                Debug.Log("并行节点没有优先级,子节点只能重新启动");
            }
        }

        public override void Clear()
        {
            base.Clear();
            successPolicy = E_Policy.One;
            failurePolicy = E_Policy.ALL;
        }
    }
}