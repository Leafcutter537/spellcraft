
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Spells
{
    [CreateAssetMenu(fileName = nameof(SpellIconDatabase), menuName = "ScriptableObjects/SpellIconDatabase")]
    public class SpellIconDatabase : ScriptableObject
    {
        [SerializeField] private List<Sprite> spellIcons;
    }
}