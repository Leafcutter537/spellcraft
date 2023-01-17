using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneDescriptionDatabase), menuName = "ScriptableObjects/RuneDescriptionDatabase")]
    public class RuneDescriptionDatabase : ScriptableObject
    {
        public List<string> runeDescriptions;

        public string GetRuneDescription(RuneType type)
        {
            int index = (int)type;
            return runeDescriptions[index];
        }
    }
}
