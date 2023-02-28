using System.Collections;
using System.Collections.Generic;
using System.Data;
using Assets.Currency;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneGenerator), menuName = "ScriptableObjects/Runes/RuneGenerator")]
    public class RuneGenerator : ScriptableObject
    {
        [SerializeField] private RuneSpriteDatabase runeSpriteDatabase;
        [SerializeField] private RuneStrengthDatabase runeStrengthDatabase;
        [SerializeField] private RuneManaCostDatabase runeManaCostDatabase;
        [SerializeField] private RuneValueDatabase runeValueDatabase;
        [SerializeField] private RuneDescriptionDatabase runeDescriptionDatabase;
        [SerializeField] private CurrencyDatabase currencyDatabase;

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
            int strength = (int)GetRuneStrength(runeData);
            int manaCost = (int)GetRuneManaCost(runeData);
            int value = (int)GetRuneValue(runeData);
            CurrencyInfo currencyInfo = currencyDatabase.GetCurrencyInfo(runeData.currencyType);
            string currencyName = currencyInfo.currencyName;
            string description = runeDescriptionDatabase.GetRuneDescription(runeData.runeType);
            Rune rune = new Rune(symbolSprite, icon, runeData, strength, manaCost, value, currencyName, description);
            return rune;
        }

        public void UpdateRuneValues(Rune rune)
        {
            RuneData runeData = rune.runeData;
            rune.strength = GetRuneStrength(runeData);
            rune.manaCost = GetRuneManaCost(runeData);
            rune.value = GetRuneValue(runeData);
            rune.description = runeDescriptionDatabase.GetRuneDescription(runeData.runeType);
        }

        private float GetRuneStrength(RuneData runeData)
        {
            RuneStrengthCoefficient coeffs = runeStrengthDatabase.GetRuneStrengthCoefficient(runeData.runeType);
            return ((coeffs.a) * Mathf.Pow(coeffs.b, runeData.rank-1) * (1 + (coeffs.c * runeData.quality / 20f)));
        }
        private float GetRuneManaCost(RuneData runeData)
        {
            RuneManaCostCoefficient coeffs = runeManaCostDatabase.GetRuneManaCostCoefficient(runeData.runeType);
            return ((coeffs.a) * Mathf.Pow(coeffs.b, runeData.rank - 1) * (1 + (coeffs.c * runeData.quality / 20f)));
        }
        private int GetRuneValue(RuneData runeData)
        {
            return (int)(GetRuneValue(runeData.runeType, runeData.rank, runeData.quality));
        }
        private int GetRuneValue(RuneType runeType, int rank, int quality)
        {
            RuneValueCoefficient coeffs = runeValueDatabase.GetRuneValueCoefficient(runeType);
            return (int)((coeffs.a) * Mathf.Pow(coeffs.b, rank - 1) * Mathf.Pow(coeffs.c, quality - 1));
        }
        public int GetRuneUpgradeCost(RuneType runeType, int rank, int currentQuality, int targetQuality)
        {
            int baseCost = GetRuneValue(runeType, rank, targetQuality) - GetRuneValue(runeType, rank, currentQuality);
            int upgradeCost = StoreConstants.GetUpgradeCost(baseCost);
            return upgradeCost;
        }
        public float GetNewManaCost(RuneData runeData, int newQuality)
        {
            RuneManaCostCoefficient coeffs = runeManaCostDatabase.GetRuneManaCostCoefficient(runeData.runeType);
            return ((coeffs.a) * Mathf.Pow(coeffs.b, runeData.rank - 1) * (1 + (coeffs.c * newQuality / 20f)));
        }
        public float GetNewStrength(RuneData runeData, int newQuality)
        {
            RuneStrengthCoefficient coeffs = runeStrengthDatabase.GetRuneStrengthCoefficient(runeData.runeType);
            return ((coeffs.a) * Mathf.Pow(coeffs.b, runeData.rank - 1) * (1 + (coeffs.c * newQuality / 20f)));
        }
    }
}