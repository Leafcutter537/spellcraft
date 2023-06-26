using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class CreateProjectile : SpellEffect
    {
        public int path;
        public int strength;
        public Element element;
        public List<ProjectileAugmentation> augmentations;
        public CreateProjectile(int path, int strength, Element element, List<ProjectileAugmentation> augmentations)
        {
            this.path = path;
            this.strength = strength;
            this.element = element;
            this.augmentations = augmentations;
        }

        public override string GetDescription()
        {
            return GetDescription(null);
        }
        public override string GetDescription(StatBundle bundle)
        {
            string returnString = "";
            string pathDescription;
            string direction;
            switch (Math.Abs(path))
            {
                case 0:
                    pathDescription = " in the chosen square.";
                    break;
                case 1:
                    pathDescription = " adjacent to the chosen square.";
                    break;
                case 2:
                    direction = path < 0 ? "below" : "above";
                    pathDescription = " two paths " + direction + " the chosen path.";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid path value in CreateProjectile.");
            }
            string bonusString = "";
            if (bundle != null)
            {
                if (bundle.projectilePower >= 0)
                    bonusString = " (+" + bundle.projectilePower + ")";
                else
                    bonusString = " (" + bundle.projectilePower + ")";
            }
            returnString = "Creates a " + element.ToString() + " projectile of strength " + strength + bonusString + pathDescription;
            foreach (ProjectileAugmentation augmentation in augmentations)
            {
                returnString = returnString + "\n" + augmentation.GetDescription();  
            }
            return returnString;
        }
    }
}
