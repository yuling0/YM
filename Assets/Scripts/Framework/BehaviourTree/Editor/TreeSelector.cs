using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;
using YMFramework.BehaviorTree;
using YMFramework.BehaviorTreeEditor;

public static class TreeSelector
{
    static BehaviorTreeController controller;
    static XN_Root root;
    [InitializeOnLoadMethod]
    private static void Init()
    {
        Selection.selectionChanged -= OnSelectionChanged;
        Selection.selectionChanged += OnSelectionChanged;
        EditorApplication.update += OnUpdate;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredEditMode && controller)
        {

        }
    }

    private static void OnUpdate()
    {
        if (EditorApplication.isPlaying)
        {
            if(controller)
            BFS(root, controller.GetRoot);
        }
    }

    private static void OnSelectionChanged()
    {
        GameObject go = Selection.activeGameObject;

        if (go && go.TryGetComponent<BehaviorTreeController>(out var tree))
        {
            if (tree.graph)
            {
                controller = tree;
                root = controller.graph.GetRoot();
                NodeEditorWindow.current.graph = tree.graph;
                //NodeEditorWindow.Open(tree.graph);
            }

        }

    }

    private static void BFS(XN_Root xnNode , Root node)
    {
        Queue<XN_NodeBase> nodeQueue1 = new Queue<XN_NodeBase>();
        Queue<Node> nodeQueue2 = new Queue<Node>();

        nodeQueue1.Enqueue(xnNode);
        nodeQueue2.Enqueue(node);

        while (nodeQueue1.Count > 0 && nodeQueue2.Count > 0)
        {
            if (nodeQueue1.Count != nodeQueue2.Count)
            {
                Debug.LogError("实时行为树 对应不上 XNode编辑器的行为树");
                return;
            }
            int size = nodeQueue1.Count;
            while (size-- > 0)
            {
                XN_NodeBase xn = nodeQueue1.Dequeue();
                Node n = nodeQueue2.Dequeue();
                xn.currentState = Extension.Parse<XN_NodeBase.State>(n.CurrentState.ToString());
                xn.currentResult = Extension.Parse<XN_NodeBase.Result>(n.CurrentResult.ToString());
                if (xn is XN_Composite)
                {
                    XN_Composite composite = xn as XN_Composite;
                    foreach (var childNode in composite.children)
                    {
                        nodeQueue1.Enqueue(childNode);
                    }
                }
                else if(xn is XN_Decorator)
                {
                    XN_Decorator decorator = xn as XN_Decorator;
                    nodeQueue1.Enqueue(decorator.decoratee);
                }

                if (n is Composite)
                {
                    Composite composite = n as Composite;
                    foreach (var childNode in composite.Children)
                    {
                        nodeQueue2.Enqueue(childNode);
                    }
                }
                else if (n is Decorator)
                {
                    Decorator decorator = n as Decorator;
                    nodeQueue2.Enqueue(decorator.CurrentDecorator);
                }
            }
        }
    }
}
