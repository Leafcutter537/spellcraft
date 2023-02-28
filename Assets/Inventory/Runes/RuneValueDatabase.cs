using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneValueDatabase), menuName = "ScriptableObjects/Runes/RuneValueDatabase")]
    public class RuneValueDatabase : ScriptableObject
    {
        public List<RuneValueCoefficient> runeValueCoefficients;

        public RuneValueCoefficient GetRuneValueCoefficient(RuneType type)
        {
            int index = (int)type;
            return runeValueCoefficients[index];
        }
    }
}
