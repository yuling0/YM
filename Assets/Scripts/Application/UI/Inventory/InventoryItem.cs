using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public enum E_ItemType
{
    Weapon,
    Prop,
}
public abstract class InventoryItem : ScriptableObject
{
    protected const string LeftVerticalGroup = "Split/Left";

    [PreviewField(55)]
    [VerticalGroup(LeftVerticalGroup)]
    [HorizontalGroup(LeftVerticalGroup + "/General Settings/Split", 55), HideLabel]
    public Sprite icon;

    [BoxGroup(LeftVerticalGroup + "/General Settings")]
    [VerticalGroup(LeftVerticalGroup + "/General Settings/Split/Right")]
    public int id;

    [VerticalGroup(LeftVerticalGroup + "/General Settings/Split/Right")]
    public string itemName;

    [VerticalGroup(LeftVerticalGroup + "/General Settings/Split/Right")]
    [ValueDropdown("ItemTypes")]
    public E_ItemType type;

    private IEnumerable<E_ItemType> ItemTypes = Enum.GetValues(typeof(E_ItemType)) as E_ItemType[];

    [HorizontalGroup("Split",0.5f,LabelWidth = 130)]
    [VerticalGroup("Split/Right")]
    [BoxGroup("Split/Right/Description"),Multiline(4),HideLabel]
    public string Description;


    [BoxGroup(LeftVerticalGroup + "/Stats")]
    public int price;

    [BoxGroup(LeftVerticalGroup + "/Stats")]
    public int maxCapacity;
}
