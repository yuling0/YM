using Sirenix.OdinInspector.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public static class ScriptableObjectCreater
{
    public static void ShowDialog<T>(string path,UnityAction<T> callback = null) where T : ScriptableObject
    {
        var selector = new  ScriptableObjectSelector<T>(path,callback);
        selector.ShowInPopup(200);
    }

    private class ScriptableObjectSelector<T> : OdinSelector<Type> where T : ScriptableObject
    {
        private string _path;
        private UnityAction<T> _callback;
        public ScriptableObjectSelector(string path, UnityAction<T> callback) : base()
        {
            _path = path;
            _callback = callback;
            this.SelectionConfirmed += CreateFireDialog;
        }


        protected override void BuildSelectionTree(OdinMenuTree tree)
        {
            var curAssembly = typeof(T).Assembly;
            var types = curAssembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(T)));
            tree.Config.DrawSearchToolbar = true;
            tree.Selection.SupportsMultiSelect = false;
            tree.AddRange(types, t => t.Name);

        }


        private void CreateFireDialog(IEnumerable<Type> selection)
        {
            var obj = ScriptableObject.CreateInstance(selection.FirstOrDefault()) as T;
            var path = _path.TrimEnd('/');

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                AssetDatabase.Refresh();
            }

            path = EditorUtility.SaveFilePanel("Save object as", path, "New" + typeof(T).Name, "asset");        //保存文件面板

            if (!string.IsNullOrEmpty(path) && TryMakeRelative(Path.GetDirectoryName(Application.dataPath),path,out path))   //因为AssetDatabase.CreateAsset方法需要相对路径
            {
                AssetDatabase.CreateAsset(obj, path);
                AssetDatabase.Refresh();

                _callback?.Invoke(obj);
            }
            else
            {
                GameObject.DestroyImmediate(obj);
            }
        }

        private bool TryMakeRelative(string absoluteParentPath, string absolutePath, out string relativePath)
        {
            //Path.GetDirectoryName方法路径是\分割的
            absoluteParentPath = absoluteParentPath.Replace(@"\","/");
            if (absolutePath.IndexOf(absoluteParentPath) < 0)
            {
                relativePath = "";
                return false;
            }
            //这里跳过一个 / 
            relativePath = absolutePath.Substring(absoluteParentPath.Length + 1, absolutePath.Length - absoluteParentPath.Length - 1);

            return true;
        }
    }
}
