using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeEditor(typeof(EndNode))]
public class EndNodeEditor : XNodeEditor.NodeEditor
{
    public override void OnBodyGUI()
    {
        base.OnBodyGUI();
        GUILayout.Label("结束对话节点",NodeEditorResources.styles.nodeHeader);
    }
}
