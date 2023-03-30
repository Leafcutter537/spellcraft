using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Combat
{
    [Serializable]
    public class EnemySpellPattern
    {
        public string spellName;
        [SerializeField] protected int cooldown;
        [SerializeField] protected int chargeTime;
        [SerializeField] public bool preventsOthersWhenCast;
        public int maxNumberCasts;
        public int currentCooldown { get; private set; }

        public void InitiateBattle()
        {
            currentCooldown = chargeTime;
        }
        public bool IsOffCooldown()
        {
            return currentCooldown == 0;
        }
        public void SetCastCooldown()
        {
            currentCooldown = cooldown;
        }
        public void DecrementCooldown()
        {
            if (cooldown > 0)
                currentCooldown--;
        }
    }
}

