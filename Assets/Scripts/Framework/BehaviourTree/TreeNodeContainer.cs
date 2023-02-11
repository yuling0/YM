using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YMFramework.BehaviorTree;

namespace YMFramework.BehaviorTreeEditor
{
    [CreateAssetMenu(menuName = "YM/BehaviorTree", fileName = "New BehaviorTree Graph")]
    public class TreeNodeContainer : XNode.NodeGraph
    {
        public XN_Root root;
        public BehaviorTreeController tree;
        public void Init(BehaviorTreeController tree)
        {
            this.tree = tree;
            if (root == null)
            {
                Debug.LogError("该行为树没有根节点");
                return;
            }

            foreach (var node in nodes)
            {
                (node as XN_NodeBase).InitNode();
            }
        }

        public void Start()
        {
            if (root == null)
            {
                Debug.LogError("该行为树没有根节点");
                return;
            }
            root.Start();
        }

        public void Stop()
        {
            if (root == null)
            {
                Debug.LogError("该行为树没有根节点");
                return;
            }
            root.Stop();
        }
        public T GetComponent<T>()
        {

            return tree.GetComponent<T>();
        }

        public T GetComponentInCore<T>() where T : ComponentBase
        {
            return tree.GetComponentInCore<T>();
        }

        public Delegate GetDelegate(string functionName)
        {
            return tree.GetDelegate(functionName);
        }

        public XN_Root GetRoot()
        {
            XN_Root root = null;
            foreach (var node in nodes)
            {
                if (node is XN_Root && (node as XN_Root).parent == null)
                {
                    if (root != null)
                    {
                        Debug.LogError("存在两个即以上的根节点（父节点为空的根节点）");
                    }
                    root = node as XN_Root;
                }
            }

            if (root == null)
            {
                Debug.LogError("没找到一个有效的根节点（父节点必须为空）");
            }
            return root;
        }
        
    }
}


