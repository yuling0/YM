using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="YM/Inventory Items/Weapon",fileName ="NewWeapon")]
public class Weapon : InventoryItem
{
    [BoxGroup(LeftVerticalGroup + "/Stats")]
    public int atk;
}
