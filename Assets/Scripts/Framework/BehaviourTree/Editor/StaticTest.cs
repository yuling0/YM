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
    //    Debug.Log("Ŷ����");
    //    EditorApplication.update += OnUpdate;//�����ڱ༭ģʽ��֡����
    //}

    private static void OnUpdate()
    {
        Debug.Log("�ڱ༭ģʽ��������");
    }

    [InitializeOnLoadMethod]
    private static void OnInit()
    {
        Debug.Log("Unity���ؿ�ʼ");


    }
}
