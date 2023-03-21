using System;
using System.Collections.Generic;
using Assets.Combat.Enemy;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(EnemySpellGenerator), menuName = "ScriptableObjects/EnemySpellGenerator")]
public class EnemySpellGenerator : ScriptableObject
{
    public List<Spell> CreateSpellList(List<EnemySpellData> enemySpellData)
    {
        List<Spell> returnList = new List<Spell>();
        foreach (EnemySpellData spellData in enemySpellData)
        {
            returnList.Add(CreateSpell(spellData));
        }
        return returnList;
    }

    public Spell CreateSpell(EnemySpellData enemySpellData)
    {
        List<SpellEffect> spellEffects = new List<SpellEffect>();
        foreach (EnemySpellEffect enemySpellEffect in enemySpellData.spellEffects)
        {
            spellEffects.Add(CreateSpellEffect(enemySpellEffect));
        }
        Spell returnSpell = new Spell(spellEffects, enemySpellData.manaCost, enemySpellData.chargeTime, enemySpellData.cooldown, enemySpellData.targetType) ;
        returnSpell.title = enemySpellData.spellName;
        returnSpell.icon = enemySpellData.icon;
        return returnSpell;
    }

    private SpellEffect CreateSpellEffect(EnemySpellEffect enemySpellEffect)
    {
        switch (enemySpellEffect.spellEffectType)
        {
            case (SpellEffectType.CreateProjectile):
                List<ProjectileAugmentation> projectileAugmentations = CreateProjectileAugmentations(enemySpellEffect.projectileAugmentations);
                return new CreateProjectile(enemySpellEffect.path, enemySpellEffect.strength, enemySpellEffect.element, projectileAugmentations);
            case (SpellEffectType.CreateShield):
                return new CreateShield(enemySpellEffect.path, enemySpellEffect.strength, enemySpellEffect.element, enemySpellEffect.duration);
            case (SpellEffectType.Heal):
                return new Heal(enemySpellEffect.strength);
            case (SpellEffectType.Buff):
                return new ApplyBuff(enemySpellEffect.strength, enemySpellEffect.duration, enemySpellEffect.stat);
            default:
                throw new ArgumentException("Invalid enemy spell effect type");
            

        }
    }

    private List<ProjectileAugmentation> CreateProjectileAugmentations(List<EnemyProjectileAugmentation> augmentationData)
    {
        List<ProjectileAugmentation> returnList = new List<ProjectileAugmentation>();
        foreach (EnemyProjectileAugmentation augmentation in augmentationData)
        {
            switch (augmentation.type)
            {
                case ProjectileAugmentationType.ApplyDebuff:
                    returnList.Add(new ApplyDebuff(augmentation.strength, augmentation.duration, augmentation.stat));
                    break;
            }
        }
        return returnList;
    }
}
