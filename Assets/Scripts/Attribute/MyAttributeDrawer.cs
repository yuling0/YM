#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyAttributeDrawer : OdinAttributeDrawer<MyAttribute>
{
    protected override void DrawPropertyLayout(GUIContent label)
    {

        Debug.Log(Property.Tree.WeakTargets.Count);

        Debug.Log(Property.Attributes[0]);

        Debug.Log(Property.ValueEntry.TypeOfValue);

        //foreach (var item in fs)
        //{
        //    Debug.Log(item);
        //}

        foreach (var item in Property.Children)
        {
            Debug.Log(item.ValueEntry.WeakSmartValue);
        }

        //foreach (var item in Property.Tree.WeakTargets)
        //{
        //    Debug.Log(item);
        //}
        CallNextDrawer(label);
    }
}
#endif