using Assets.Combat.SpellEffects;
using System;
using UnityEngine;

namespace Assets.Combat
{
    [Serializable]
    public class EnemyShieldData
    {
        public Vector2Int coords;
        public int strength;
        public Element element;
        public int duration;
    }
}
