using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UIPanelScriptGenerater 
{

    [MenuItem("YMTools/UIGenerater/生成选中的面板")]
    public static void GenerateUIPanel()
    {
        var selectPanel = Selection.activeGameObject;
        var panelName = selectPanel.name;

        var bindingField = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/UITemplate/UIPanelBindingField.txt").text;
        var initField = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/UITemplate/UIPanelInitField.txt").text;
        var uiPanelTemplate = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/Editor/UITemplate/UIPanelTemplate.txt").text;

        var dic = new Dictionary<string, Type>();

        var childsTF = selectPanel.GetComponentsInChildren<Transform>();

        foreach (var tf in childsTF)
        {
            if(tf.name.IndexOf("img") != -1)
            {
                dic.Add(tf.name, typeof(Image));
            }
            else if(tf.name.IndexOf("txt") != -1)
            {
                dic.Add(tf.name, typeof(Text));
            }
            else if(tf.name.IndexOf("btn") != -1)
            {
                dic.Add(tf.name, typeof(Button));
            }
            else if (tf.name.IndexOf("sld") != -1)
            {
                dic.Add(tf.name, typeof(Slider));
            }
            else if (tf.name.IndexOf("tog") != -1)
            {
                dic.Add(tf.name, typeof(Toggle));
            }
            else if (tf.name.IndexOf("if") != -1)
            {
                dic.Add(tf.name, typeof(InputField));
            }
        }

        StringBuilder bindingFieldStr = new StringBuilder();
        StringBuilder initFieldStr = new StringBuilder();

        foreach (var keyValuePair in dic)
        {
            bindingFieldStr.AppendLine(bindingField.Replace("%UIType%", keyValuePair.Value.Name).Replace("%controlName%", keyValuePair.Key));
            initFieldStr.AppendLine(initField.Replace("%UIType%", keyValuePair.Value.Name).Replace("%controlName%", keyValuePair.Key));
        }

        var scriptText = uiPanelTemplate.Replace("%UIPanel%", panelName)
            .Replace("%UIPanelBindingField%", bindingFieldStr.ToString())
            .Replace("%OnInt%", initFieldStr.ToString());
        Debug.Log(Application.dataPath + $"/Scripts/Application/UI/{panelName}.cs");
        File.WriteAllText(Application.dataPath + $"/Scripts/Application/UI/UIPanel/{panelName}.cs", scriptText);
        AssetDatabase.Refresh();
    }

    [MenuItem("YMTools/UIGenerater/生成选中的面板",true)]
    public static bool SelectPanelVerification()
    {
        var selectPanel = Selection.activeGameObject;
        return selectPanel != null;
    }

}
