using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneGenerator), menuName = "ScriptableObjects/RuneGenerator")]
    public class RuneGenerator : ScriptableObject
    {
        [SerializeField] private RuneSpriteDatabase runeSpriteDatabase;
        [SerializeField] private RuneStrengthDatabase runeStrengthDatabase;
        [SerializeField] private RuneManaCostDatabase runeManaCostDatabase;
        [SerializeField] private RuneDescriptionDatabase runeDescriptionDatabase;

        public List<Rune> CreateRunes(List<RuneData> runeDataList)
        {
            List<Rune> runes = new List<Rune>();
            foreach (RuneData runeData in runeDataList)
            {
                if (runeData != null)
                    runes.Add(CreateRune(runeData));
                else
                    runes.Add(null);
            }
            return runes;
        }

        public Rune CreateRune(RuneData runeData)
        {
            Sprite symbolSprite = runeSpriteDatabase.GetSymbolSprite(runeData.runeType);
            Sprite icon = runeSpriteDatabase.GetRankShapeSprite(runeData.rank);
            float strength = GetRuneStrength(runeData);
            float manaCost = GetRuneManaCost(runeData);
            string description = runeDescriptionDatabase.GetRuneDescription(runeData.runeType);
            Rune rune = new Rune(symbolSprite, icon, runeData, strength, manaCost, description);
            return rune;
        }

        private float GetRuneStrength(RuneData runeData)
        {
            RuneStrengthCoefficient coeffs = runeStrengthDatabase.GetRuneStrengthCoefficient(runeData.runeType);
            return (coeffs.a) * ((Mathf.Pow(coeffs.b, runeData.rank-1)) * (1 + (coeffs.c * runeData.quality / 100f)));
        }
        private float GetRuneManaCost(RuneData runeData)
        {
            RuneManaCostCoefficient coeffs = runeManaCostDatabase.GetRuneManaCostCoefficient(runeData.runeType);
            return (coeffs.a) * ((Mathf.Pow(coeffs.b, runeData.rank - 1)) * (1 + (coeffs.c * runeData.quality / 100f)));
        }
    }
}