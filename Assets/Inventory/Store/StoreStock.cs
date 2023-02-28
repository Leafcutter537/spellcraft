
using System.Collections.Generic;
using System.ComponentModel;
using Assets.Inventory.Runes;
using UnityEngine;

namespace Assets.Store
{
    [CreateAssetMenu(fileName = nameof(StoreStock), menuName = "ScriptableObjects/Store/StoreStock")]
    public class StoreStock : ScriptableObject
    {
        [SerializeField] private RuneGenerator runeGenerator;
        [SerializeField] private List<RuneData> baseStoreStock;
        public List<RuneData> runeStoreStock;

        public List<SelectChoice> GetRuneStoreStock()
        {
            List<SelectChoice> returnList = new List<SelectChoice>();
            List<Rune> runes = runeGenerator.CreateRunes(runeStoreStock);
            foreach (Rune rune in runes)
            {
                returnList.Add(rune as SelectChoice);
            }
            return returnList;
        }

        public void CopyBase()
        {
            runeStoreStock = new List<RuneData>();
            foreach (RuneData rune in baseStoreStock)
            {
                runeStoreStock.Add(new RuneData(rune));
            }
        }
    }
}
