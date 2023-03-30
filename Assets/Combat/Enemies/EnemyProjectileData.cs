using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using UnityEngine;

namespace Assets.Combat
{
    [Serializable]
    public class EnemyProjectileData
    {
        public Vector2Int coords;
        public int strength;
        public Element element;
        public List<EnemyProjectileAugmentation> augmentations;
    }
}
