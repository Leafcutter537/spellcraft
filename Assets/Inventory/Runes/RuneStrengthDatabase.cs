using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneStrengthDatabase), menuName = "ScriptableObjects/RuneStrengthDatabase")]
    public class RuneStrengthDatabase : ScriptableObject
    {
        public List<RuneStrengthCoefficient> runeStrengthCoefficients;

        public RuneStrengthCoefficient GetRuneStrengthCoefficient(RuneType type)
        {
            int index = (int)type;
            return runeStrengthCoefficients[index];
        }
    }
}
