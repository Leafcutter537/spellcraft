using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(DevSpellInventory), menuName = "ScriptableObjects/DevSpellInventory")]
    public class DevSpellInventory : ScriptableObject
    {
        public List<SpellData> firstSpellList;
        public List<SpellData> secondSpellList;
    }
}
