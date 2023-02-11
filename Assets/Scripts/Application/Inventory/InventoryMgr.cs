using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMgr : SingletonBase<InventoryMgr>
{
    private Dictionary<int, List<InventoryItemSlot>> _itemSlotDic = new Dictionary<int, List<InventoryItemSlot>>();
    private List<InventoryItemSlot> _itemSlots = new List<InventoryItemSlot>();

    private int _maxCapacity = 48;

    public List<InventoryItemSlot> GetInventoryItems 
    {
        get
        {
            //按id排序
            _itemSlots.Sort((a, b) => { return a.Id - b.Id; });
            return _itemSlots;
        }
    }
    /// <summary>
    /// 获取背包最大页数 - 1
    /// </summary>
    /// <param name="perPageCount">每页多少个物品</param>
    /// <returns></returns>
    public int GetMaxPage(int perPageCount)
    {
        if (_itemSlots.Count == 0) return 0;
        return _itemSlots.Count % perPageCount != 0 ? _itemSlots.Count / perPageCount : _itemSlots.Count / perPageCount - 1;
    }

    /// <summary>
    /// 添加道具
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool AddItem(int id)
    {
        if (_itemSlots.Count >= _maxCapacity)
        {
            Debug.Log("背包已满，无法再装下了");
            return false;
        }

        bool flag = true;
        if(_itemSlotDic.ContainsKey(id))
        {
            foreach (var item in _itemSlotDic[id])
            {
                if(!item.IsFull)
                {
                    item.Add();
                    flag = false;
                    break;
                }
            }
        }

        if(flag)
        {
            _itemSlots.Add(new InventoryItemSlot(id));
        }
        return true;
    }

    /// <summary>
    /// 使用道具后移除道具
    /// </summary>
    /// <param name="index"></param>
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= _itemSlots.Count) return;
        InventoryItemSlot slot = _itemSlots[index];

        if(--slot.Count <= 0)
        {
            _itemSlots.RemoveAt(index);
        }
    }

}
