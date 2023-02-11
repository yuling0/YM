using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 物品图鉴类
/// </summary>
public class IllustratedHandbook : SingletonBase<IllustratedHandbook>
{
    private Dictionary<int, InventoryItem> itemDic = new Dictionary<int, InventoryItem>();          //根据ID查找物品

    [TableList(ShowPaging = true, NumberOfItemsPerPage = 6),ShowInInspector]
    private List<InventoryItemWarpper> allItems = new List<InventoryItemWarpper>();                 //物品列表用于显示在YMEditor Window

    private IllustratedHandbook()
    {
//#if UNITY_EDITOR
//        //初始化字典
//        var items = AssetDatabase.FindAssets("t:InventoryItem")
//            .Select((x) =>
//            {
//                return AssetDatabase.LoadAssetAtPath<InventoryItem>(AssetDatabase.GUIDToAssetPath(x));
//            });
//#else
        var items = ResourceMgr.Instance.LoadAllAsset<InventoryItem>("ScriptableObjects/InventoryItems");
//#endif

        foreach (var item in items)
        {
            if(!itemDic.ContainsKey(item.id))
            {
                itemDic.Add(item.id, item);
            }
        }
    }

    public InventoryItem GetItemInId(int id)
    {
        if(itemDic.ContainsKey(id))
        {
            return itemDic[id];
        }
        Debug.LogError($"未找到id为：{id}的物品");
        return null;
    }
    /// <summary>
    /// 加载工程中所有Inventory Item
    /// </summary>
    #if UNITY_EDITOR
    public void LoadItems()
    {

        var items = AssetDatabase.FindAssets("t:InventoryItem");

        allItems = items.Select((x) =>
        {
            return new InventoryItemWarpper(AssetDatabase.LoadAssetAtPath<InventoryItem>(AssetDatabase.GUIDToAssetPath(x)));
        }).ToList();
    }
#endif
    /// <summary>
    /// 物品包装器：用于显示在YMEditor Window
    /// </summary>
    private class InventoryItemWarpper
    {

        public InventoryItemWarpper(InventoryItem item)
        {
            this.icon = item.icon;
            this.id = item.id;
            this.itemName = item.itemName;
            this.type = item.type;
        }
        [PreviewField(55)]
        [TableColumnWidth(55 , Resizable = false)]
        public Sprite icon;

        public int id;

        public string itemName;

        public E_ItemType type;

    }
}
