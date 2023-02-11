using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//物品背包格子
public class InventoryItemSlot
{
    private InventoryItem _curItem;
    private int _id;
    private int _count;
    public InventoryItemSlot(int id)
    {
        _curItem = IllustratedHandbook.Instance.GetItemInId(id);
        _count = 1;
        _id = id;
    }
    public InventoryItem GetItem => _curItem;
    public int Id => _id;
    public bool IsFull => _count >= _curItem.maxCapacity;
    public int Count 
    {
        set
        {
            _count = value;
        }
        get
        {
            return _count;
        }
    }
    public bool Add()
    {
        if (IsFull) return false;

        _count++;

        return true;
    }

}
