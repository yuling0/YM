//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Reflection;
//using System.Text;
//using UnityEditor;
//using UnityEngine;
//using YMFramework.BehaviorTree;
//public class TreeNodeAssetPostprocessor : AssetPostprocessor
//{
//    static string TreeNodeTemplateText;
//    static string InitNodeFiledTemplateText;
//    static string TreeNodeFieldTemplateText;
//    static string TreeNodeTemplatePath = "Assets/Scripts/Framework/BehaviourTree/Editor/TreeNodeTemplate/TreeNodeTemplate.txt";
//    static string InitNodeFiledTemplatePath = "Assets/Scripts/Framework/BehaviourTree/Editor/TreeNodeTemplate/InitNodeFiledTemplate.txt";
//    static string TreeNodeFieldTemplatePath = "Assets/Scripts/Framework/BehaviourTree/Editor/TreeNodeTemplate/TreeNodeFieldTemplate.txt";
//    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
//    {
//        foreach (string assetPath in importedAssets)
//        {
//            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(assetPath);
//            if (script != null  && !script.GetClass().IsAbstract && typeof(Node).IsAssignableFrom(script.GetClass()))
//            {
//                Type nodeType = script.GetClass();
//                if (string.IsNullOrEmpty(TreeNodeTemplateText))
//                {
//                    TreeNodeTemplateText = AssetDatabase.LoadAssetAtPath<TextAsset>(TreeNodeTemplatePath).text;
//                    InitNodeFiledTemplateText = AssetDatabase.LoadAssetAtPath<TextAsset>(InitNodeFiledTemplatePath).text;
//                    TreeNodeFieldTemplateText = AssetDatabase.LoadAssetAtPath<TextAsset>(TreeNodeFieldTemplatePath).text;
//                }
//                StringBuilder NodeField = new StringBuilder();
//                StringBuilder InitNodeFiled = new StringBuilder();
//                FieldInfo [] fieldInfos = nodeType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
//                foreach (var info in fieldInfos)
//                {
//                    NodeField.AppendLine(TreeNodeFieldTemplateText.Replace("%FieldType%",info.FieldType.FullName).Replace("%FieldName%",info.Name));
//                    InitNodeFiled.AppendLine(InitNodeFiledTemplateText.Replace("%FieldName%", info.Name));
//                }
//                string scriptText = TreeNodeTemplateText.Replace("%TreeNodeName%", nodeType.Name)
//                    .Replace("%ParentClassName%", nodeType.BaseType.Name)
//                    .Replace("%NodeField%", NodeField.ToString())
//                    .Replace("%Init%", InitNodeFiled.ToString());

//                File.WriteAllText(Application.dataPath + $"/Scripts/Framework/BehaviourTree/Gen/XN_{nodeType.Name}.cs", scriptText);
//            }
//        }
//        AssetDatabase.Refresh();
//    }
//}
