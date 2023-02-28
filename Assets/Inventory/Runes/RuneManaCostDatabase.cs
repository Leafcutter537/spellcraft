using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneManaCostDatabase), menuName = "ScriptableObjects/Runes/RuneManaCostDatabase")]
    public class RuneManaCostDatabase : ScriptableObject
    {
        public List<RuneManaCostCoefficient> runeManaCostCoefficients;

        public RuneManaCostCoefficient GetRuneManaCostCoefficient(RuneType type)
        {
            int index = (int)type;
            return runeManaCostCoefficients[index];
        }
    }
}
