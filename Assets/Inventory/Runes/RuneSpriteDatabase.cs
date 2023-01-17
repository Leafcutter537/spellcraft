
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Inventory.Runes
{
    [CreateAssetMenu(fileName = nameof(RuneSpriteDatabase), menuName = "ScriptableObjects/RuneSpriteDatabase")]
    public class RuneSpriteDatabase : ScriptableObject
    {
        [SerializeField] private List<Sprite> symbolSprites;
        [SerializeField] private List<Sprite> rankShapeSprites;

        public Sprite GetSymbolSprite(RuneType type)
        {
            int index = (int)type;
            return symbolSprites[index];
        }

        public Sprite GetRankShapeSprite(int rank)
        {
            int index = rank - 1;
            return rankShapeSprites[index];
        }
    }

}
