using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    public Image _imgIcon;
    public Text _txtName;
    public Text _txtCount;

    public void Init(InventoryItemSlot slot)
    {

        _imgIcon = transform.GetControl<Image>("img_Icon");
        _txtName = transform.GetControl<Text>("txt_Name");
        _txtCount = transform.GetControl<Text>("txt_Count");

        _imgIcon.sprite = slot.GetItem.icon;
        _txtName.text = slot.GetItem.itemName;
        _txtCount.text = string.Format($"× {slot.Count}");
    }
}
