using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace YMFramework.BehaviorTreeEditor
{
    [CustomNodeEditor(typeof(XN_NodeBase))]
    public class TreeNodeEditor : XNodeEditor.NodeEditor
    {
        XN_NodeBase curNode;
        public override void OnCreate()
        {
            base.OnCreate();
            curNode = target as XN_NodeBase;
        }
        public override void OnBodyGUI()
        {
            NodePort input = curNode.GetInputPort("input");
            NodePort output = curNode.GetOutputPort("output");
            
            NodeEditorGUILayout.DrawTreeNodePort(new GUIContent("In"), input);

            //YMFramework.BehaviorTree.Node node = curNode as YMFramework.BehaviorTree.Node;

            GUILayout.Label(curNode.nodeDescription,NodeEditorResources.styles.nodeHeader);

            NodeEditorGUILayout.DrawTreeNodePort(new GUIContent("Ou"), output);


        }

        public override Color GetTint()
        {
            return curNode.GetColor();
        }
    }
}

