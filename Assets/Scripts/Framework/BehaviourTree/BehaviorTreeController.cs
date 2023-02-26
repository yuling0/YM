using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;
using YMFramework.BehaviorTreeEditor;

namespace YMFramework.BehaviorTree
{
    /// <summary>
    /// 行为树管理类
    /// </summary>
    public class BehaviorTreeController : ComponentBase
    {
        public TreeNodeContainer graph; // 所挂载的行为树实例（主要用于创建行为树）
        protected Root tree;    //生成的行为树根节点
        protected Dictionary<string, Delegate> functionDic;   //行为树某些节点需要绑定的方法（例如Condition节点需要执行的方法）
        protected Blackboard blackboard;    //该行为树的黑板
        protected GlobalClock clock;    //全局时钟，用于计时通知
        public sealed override void Init(Core core, object userData)
        {
            base.Init(core,userData);

            functionDic = new Dictionary<string, Delegate>();
            InitTree();
            InitFunctionMap();
            clock = GlobalClock.Instance;
            tree = BehaviorTreeCreator.Instance.CreateTree(graph);
            tree.InitNode(this);
            blackboard = tree.Blackboard;
        }

        protected virtual void InitTree()
        {
            
        }
        /// <summary>
        /// 初始化方法映射
        /// </summary>
        protected virtual void InitFunctionMap()
        {

        }
        public override void OnEnableComponent()
        {
            BehaviorStart();
        }

        public override void OnDisableComponent()
        {
            BehaviorStop();
        }
        public  void BehaviorStart()
        {
            tree.Start();
        }

        public void BehaviorStop()
        {
            tree.Stop();
        }

        protected void AddDelegate(string functionName,Delegate functionDelegate)
        {
            functionDic.Add(functionName, functionDelegate);
        }
        public virtual Delegate GetDelegate(string functionName)
        {
            if (!functionDic.ContainsKey(functionName))
            {
                Debug.LogError($"{this.GetType().Name} 类中未找到对应的方法：{functionName}");
                return null;
            }
            return functionDic[functionName];
        }

        public T GetDelegate<T>(string functionName) where T : Delegate
        {
            if (!functionDic.ContainsKey(functionName))
            {
                Debug.LogError($"{this.GetType().Name} 类中未找到对应的方法：{functionName}");
                return null;
            }
            return functionDic[functionName] as T;
        }

#if UNITY_EDITOR
        public Root GetRoot => tree;
#endif
    }
}

