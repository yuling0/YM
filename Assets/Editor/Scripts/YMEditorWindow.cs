using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class YMEditorWindow : OdinMenuEditorWindow
{
    [MenuItem("YMTools/YMEditorWindow")]
    private static void Open()
    {
        var window = GetWindow<YMEditorWindow>();
        window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 600);
    }
    protected override OdinMenuTree BuildMenuTree()
    {

        var tree = new OdinMenuTree(true);

        IllustratedHandbook.Instance.LoadItems();

        tree.AddAllAssetsAtPath("Arche", Consts.P_ScriptableObjectsPath,typeof(ArcheInfo), true,true);

        tree.AddAllAssetsAtPath("Monster", Consts.P_ScriptableObjectsPath, typeof(MonsterInfo), true,true);

        tree.Add("IllstratedHandbook", IllustratedHandbook.Instance);

        tree.AddAllAssetsAtPath("Weapon", Consts.P_ScriptableObjectsPath, typeof(Weapon),true,true);

        tree.AddAllAssetsAtPath("Prop", Consts.P_ScriptableObjectsPath, typeof(Prop), true, true);

        tree.EnumerateTree().AddIcons<CombatInfo>(x => x.icon);      //枚举OdinMenuTree中的内容，并为CharacterInfo添加图标

        tree.EnumerateTree().AddIcons<InventoryItem>(x => x.icon);      //枚举OdinMenuTree中的内容，并为InventoryItem添加图标
        return tree;
    }

    protected override void OnBeginDrawEditors()
    {
        var firstSelect = this.MenuTree.Selection.FirstOrDefault();
        var height = this.MenuTree.Config.SearchToolbarHeight;

        SirenixEditorGUI.BeginHorizontalToolbar(height);
        {
            if(firstSelect != null)
            {
                GUILayout.Label(firstSelect.Name);
            }

            if(SirenixEditorGUI.ToolbarButton("Create Inventory Item"))
            {
                ScriptableObjectCreater.ShowDialog<InventoryItem>(Consts.P_ScriptableObjectsPath + "/InventoryItems", (item) =>
                 {
                     base.TrySelectMenuItemWithObject(item);
                 });
            }

            if (SirenixEditorGUI.ToolbarButton("Create MonsterInfo"))
            {
                ScriptableObjectCreater.ShowDialog<MonsterInfo>(Consts.P_ScriptableObjectsPath + "/MonsterInfo", (obj) =>
                {
                    base.TrySelectMenuItemWithObject(obj);
                });
            }
        }
        SirenixEditorGUI.EndHorizontalToolbar();
    }

}
