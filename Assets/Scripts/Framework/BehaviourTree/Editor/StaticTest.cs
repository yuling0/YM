using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[InitializeOnLoad]
public static class StaticTest
{
    //static StaticTest()
    //{
    //    Debug.Log("哦呦呦");
    //    EditorApplication.update += OnUpdate;//类似在编辑模式的帧更新
    //}

    private static void OnUpdate()
    {
        Debug.Log("在编辑模式持续调用");
    }

    [InitializeOnLoadMethod]
    private static void OnInit()
    {
        Debug.Log("Unity加载开始");


    }
}
