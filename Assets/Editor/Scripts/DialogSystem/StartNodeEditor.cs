using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(StartNode))]
public class StartNodeEditor : XNodeEditor.NodeEditor
{
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        GUILayout.Label("开始对话节点", NodeEditorResources.styles.nodeHeader);
    }
}
