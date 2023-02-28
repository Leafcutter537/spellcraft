using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(DevSpellInventory), menuName = "ScriptableObjects/Spells/DevSpellInventory")]
    public class DevSpellInventory : ScriptableObject
    {
        public List<SpellData> firstSpellList;
        public List<SpellData> secondSpellList;
    }
}
