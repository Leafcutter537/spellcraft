using System.Collections;
using System.Collections.Generic;
using Assets.Combat.SpellEffects;
using Assets.Inventory.Spells;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Combat
{
    public class EnemyAI : MonoBehaviour
    {
        private List<EnemyProjectilePattern> projectilePatterns;
        private List<EnemyShieldPattern> shieldPatterns;
        private List<EnemyHealPattern> healPatterns;
        private List<EnemyBuffPattern> buffPatterns;
        private List<EnemySpellPattern> allPatterns;
        [SerializeField] private EnemySpellGenerator enemySpellGenerator;
        private int spellIndex;
        private SpellcastingStage spellcastingStage;
        private int numSpellCast;
        [SerializeField] private EnemyInstance enemyInstance;
        [SerializeField] private PathController pathController;
        [SerializeField] private int[] pathPriority;


        public void SetPatterns(List<EnemyProjectilePattern> projectilePatterns, List<EnemyShieldPattern> shieldPatterns,
            List<EnemyHealPattern> healPatterns, List<EnemyBuffPattern> buffPatterns)
        {
            this.projectilePatterns = projectilePatterns;
            this.shieldPatterns = shieldPatterns;
            this.healPatterns = healPatterns;
            this.buffPatterns = buffPatterns;

            allPatterns = new List<EnemySpellPattern>();

            foreach (EnemyProjectilePattern projectilePattern in projectilePatterns)
                allPatterns.Add(projectilePattern);
            foreach (EnemyShieldPattern shieldPattern in shieldPatterns)
                allPatterns.Add(shieldPattern);
            foreach (EnemyHealPattern healPattern in healPatterns)
                allPatterns.Add(healPattern);
            foreach (EnemyBuffPattern buffPattern in buffPatterns)
                allPatterns.Add(buffPattern);

            foreach (EnemySpellPattern spell in allPatterns)
                spell.InitiateBattle();
        }

        public bool PerformNextSpell()
        {
            while (spellcastingStage == SpellcastingStage.Buff)
            {
                if (buffPatterns.Count == 0)
                {
                    NextStage();
                }
                else if (AdvanceThroughSpells(buffPatterns[spellIndex], buffPatterns.Count))
                    return true;
            }
            while (spellcastingStage == SpellcastingStage.Heal)
            {
                if (healPatterns.Count == 0)
                {
                    NextStage();
                }
                else if (AdvanceThroughSpells(healPatterns[spellIndex], healPatterns.Count))
                    return true;
            }
            while (spellcastingStage == SpellcastingStage.Shield)
            {
                if (shieldPatterns.Count == 0)
                {
                    NextStage();
                }
                else if (AdvanceThroughSpells(shieldPatterns[spellIndex], shieldPatterns.Count))
                    return true;
            }
            while (spellcastingStage == SpellcastingStage.Projectile)
            {
                if (projectilePatterns.Count == 0)
                {
                    NextStage();
                }
                else if (AdvanceThroughSpells(projectilePatterns[spellIndex], projectilePatterns.Count))
                    return true;
            }
            return false;
        }

        private bool AdvanceThroughSpells(EnemySpellPattern spellToCast, int spellCount)
        {
            if (spellToCast.IsOffCooldown())
            {
                if (enemyInstance.CastSpell(spellToCast))
                {
                    numSpellCast++;
                    if (spellToCast.maxNumberCasts <= numSpellCast)
                        spellIndex++;
                    if (spellIndex >= spellCount | spellToCast.preventsOthersWhenCast)
                    {
                        NextStage();
                    }
                    return true;
                }
                else
                {
                    spellIndex++;
                    if (spellIndex >= spellCount)
                    {
                        NextStage();
                    }
                    return false;
                }
            }
            else
            {
                spellIndex++;
                if (spellIndex >= spellCount)
                {
                    NextStage();
                }
                return false;
            }
        }

        private void NextStage()
        {
            spellIndex = 0;
            numSpellCast = 0;
            switch (spellcastingStage)
            {
                case (SpellcastingStage.Buff):
                    spellcastingStage = SpellcastingStage.Heal;
                    break;
                case (SpellcastingStage.Heal):
                    spellcastingStage = SpellcastingStage.Shield;
                    break;
                case (SpellcastingStage.Shield):
                    spellcastingStage = SpellcastingStage.Projectile;
                    break;
                case (SpellcastingStage.Projectile):
                    spellcastingStage = SpellcastingStage.Finished;
                    break;
            }
        }

        public void ResetSpellcasting()
        {
            spellIndex = 0;
            spellcastingStage = SpellcastingStage.Buff;
            foreach (EnemySpellPattern spellPattern in allPatterns)
            {
                spellPattern.DecrementCooldown();
            }
        }

        private enum SpellcastingStage
        {
            Buff,
            Heal,
            Shield,
            Projectile,
            Finished
        }


    }
}
