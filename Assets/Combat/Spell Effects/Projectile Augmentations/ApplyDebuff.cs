using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;

public class ApplyDebuff : ProjectileAugmentation
{
    public int duration;
    public CombatStat stat;
    public override void EnactAugmentationEffect(CharacterInstance target)
    {
        target.AttemptApplyDebuff(this);
    }

    public override string GetDescription()
    {
        return PlayerStats.GetCombatStatName(stat) + " debuff: (-" + strength.ToString() + ") for " + duration + " turns.";
    }

    public ApplyDebuff(int strength, int duration, CombatStat stat)
    {
        this.strength = strength;
        this.duration = duration;
        this.stat = stat;
    }
}
