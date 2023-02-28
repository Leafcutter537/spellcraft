using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Runes;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(DevRuneInventory), menuName = "ScriptableObjects/Runes/DevRuneInventory")]
public class DevRuneInventory : ScriptableObject
{
    public List<RuneData> firstRuneList;
    public List<RuneData> secondRuneList;
}
