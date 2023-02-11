using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="YM/Inventory Items/Prop",fileName ="NewProp")]
public class Prop : InventoryItem
{
    [BoxGroup(LeftVerticalGroup + "/Stats")]
    public int val;

    //[BoxGroup(LeftVerticalGroup + "/Stats")]
    //public int count;

    //[BoxGroup(LeftVerticalGroup + "/Stats")]
    //public int maxCapacity;
}
