
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(SpellIconDatabase), menuName = "ScriptableObjects/Spells/SpellIconDatabase")]
    public class SpellIconDatabase : ScriptableObject
    {
        [SerializeField] private List<Sprite> spellIcons;

        public Sprite GetSpellIcon(int index)
        {
            return spellIcons[index];
        }
        public List<SelectChoice> GetItemList()
        {
            List<SelectChoice> returnList = new List<SelectChoice>();
            foreach (Sprite sprite in spellIcons)
            {
                returnList.Add(new SpellIconSelectChoice(sprite));
            }
            return returnList;
        }
    }
}