using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

public class SetPathPopUp : OdinEditorWindow
{

    [LabelText("文件名称(需要后缀)")]
    [ValidateInput("IsNotNull", "$errorMsg", InfoMessageType.Error)]
    public string fileName;

    [LabelText("文件夹名称")]
    public string directory = "AI/";

    public static object data;

    private string errorMsg;
    [Button]
    private void Confirm()
    {
        if (IsNotNull())
        {
            BinaryDataManager.Instance.SaveData(data, directory + fileName, false);

            AssetDatabase.Refresh();

            Close();
        }
    }

    public static void OpenWindow(object data,string defaultName = "")
    {
        SetPathPopUp.data = data;
        var window = GetWindow<SetPathPopUp>();
        window.fileName = defaultName;
        window.position = NodeEditorWindow.current.position.AlignCenter(300, 100);

    }
    /// <summary>
    /// 随便判断一下 （不严谨）
    /// </summary>
    /// <returns></returns>
    private bool IsNotNull()
    {
        if (fileName == null)
        {
            errorMsg = "文件名称不能为空";
            return false;
        }
        else if(fileName.Length == 0)
        {
            errorMsg = "文件名称不能为空串";
            return false;
        }
        else if(fileName.StartsWith("."))
        {
            errorMsg = "输入合法文件名称";
            return false;
        }
        else if (fileName.LastIndexOf('.') == -1 || fileName.LastIndexOf('.') == fileName.Length -1)
        {
            errorMsg = "输入合法文件名称";
            return false;
        }

        return true;
    }
}
