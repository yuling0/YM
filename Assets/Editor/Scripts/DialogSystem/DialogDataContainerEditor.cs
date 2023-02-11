using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNodeEditor;

[CustomNodeGraphEditor(typeof(DialogDataContainer),"DialogNodeSettings")]
public class DialogDataContainerEditor : NodeGraphEditor
{
    public override string GetNodeMenuName(Type type)
    {
        if (typeof(DialogNodeBase).IsAssignableFrom(type))
        {
            return type.Name;
        }
        return null;
    }

    public override NodeEditorPreferences.Settings GetDefaultPreferences()
    {
        var settings = new NodeEditorPreferences.Settings();
        settings.noodlePath = NoodlePath.Curvy;
        settings.gridLineColor = new Color32(98, 224, 156, 255);
        settings.typeColors = new Dictionary<string, Color>() 
        { 
            { typeof(bool).PrettyName(), new Color32(98, 224, 156, 255) }
        };
        return settings;
    }
}
