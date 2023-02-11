using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.Serialization;
using System.IO;
using YMFramework.BehaviorTree;
using XNodeEditor;

namespace YMFramework.BehaviorTreeEditor
{
    [CustomNodeGraphEditor(typeof(TreeNodeContainer),"TreeNodeSettings")]
    public class BehaviorTreeGraphEditor : NodeGraphEditor
    {
        TreeNodeContainer treeNodeGraph;
        public override void OnCreate()
        {
            treeNodeGraph = target as TreeNodeContainer;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }



        public override void OnGUI()
        {
            if(GUI.Button(new Rect(0,0,100,30),"保存行为树"))
            {
                TreeData treeData = new TreeData();

                XN_Root root = treeNodeGraph.GetRoot();

                if (root is null)
                {
                    Debug.LogError("根节点为空,无法生成行为树");
                    return;
                }

                BFS(root, treeData);

                SetPathPopUp.OpenWindow(treeData, treeNodeGraph.name + ".ling"); //默认后缀为.ling（可修改）

            }

        }

        private void OnPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingEditMode)
            {
                AutoSave();
            }

        }

        /// <summary>
        /// 自动保存行为树的修改，文件名会为ScriptableObject的文件名 
        /// </summary>
        public void AutoSave()
        {
            TreeData treeData = new TreeData();

            XN_Root root = treeNodeGraph.GetRoot();

            if (root is null)
            {
                Debug.LogError("根节点为空,无法生成行为树");
                return;
            }

            BFS(root, treeData);

            string path = "AI/" + treeNodeGraph.name + ".ling";

            BinaryDataManager.Instance.SaveData<TreeData>(treeData, path, false);

        }
        private void BFS(XN_Root root,TreeData treeData)
        {
            Queue<XN_NodeBase> nodes = new Queue<XN_NodeBase>();
            int id = 0;
            nodes.Enqueue(root);
            while (nodes.Count > 0 )
            {
                int size = nodes.Count;
                while (size-- > 0)
                {
                    XN_NodeBase n = nodes.Dequeue();
                    NodeDataBase nodeDataBase = n.CreateNodeData(id++);
                    treeData.data.Add(nodeDataBase);

                    if (n is XN_Composite)
                    {
                        XN_Composite composite = n as XN_Composite;
                        composite.children.Sort((a, b) => 
                        {
                            return a.position.x < b.position.x ? -1 : 1;
                        });
                        for (int i = 0; i < composite.children.Count; i++)
                        {
                            nodeDataBase.AddChild(id + nodes.Count);
                            nodes.Enqueue(composite.children[i]);
                        }
                    }
                    else if(n is XN_Decorator)
                    {
                        XN_Decorator decorator = n as XN_Decorator;
                        if (decorator.decoratee != null)
                        {
                            nodeDataBase.AddChild(id + nodes.Count);
                            nodes.Enqueue(decorator.decoratee);
                            
                        }
                    }
                }
            }
        }
        public override string GetNodeMenuName(Type type)
        {
            if(typeof(XN_NodeBase).IsAssignableFrom(type))
            {
                if (typeof(XN_Composite).IsAssignableFrom(type))
                {
                    return $"组合节点/{type.Name}";
                }
                else if (typeof(XN_Decorator).IsAssignableFrom(type))
                {
                    return $"装饰节点/{type.Name}";
                }
                else if (typeof(XN_Task).IsAssignableFrom(type))
                {
                    return $"行为节点/{type.Name}";
                }
                return type.Name;
            }
            return null;
            
        }
        public override NodeEditorPreferences.Settings GetDefaultPreferences()
        {
            var settings = new NodeEditorPreferences.Settings();
            settings.noodlePath = NoodlePath.Straight;
            settings.typeColors = new Dictionary<string, Color>()
        {
            { typeof(int).PrettyName(), new Color32(98, 224, 156, 255) }
        };
            return settings;
        }

    }
}

