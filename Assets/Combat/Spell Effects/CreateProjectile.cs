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
        public CreateProjectile(int path, int strength, Element element)
        {
            this.path = path;
            this.strength = strength;
            this.element = element;
        }

        public override string GetDescription()
        {
            return GetDescription(null);
        }
        public override string GetDescription(StatBundle bundle)
        {
            string pathDescription;
            string direction;
            switch (path)
            {
                case 0:
                    pathDescription = " along the chosen path.";
                    break;
                case 1:
                    direction = path < 0 ? "below" : "above";
                    pathDescription = " one path " + direction + " the chosen path.";
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
                bonusString = " (+" + bundle.projectilePower + ")";
            }
            return "Creates a " + element.ToString() + " projectile of strength " + strength + bonusString + pathDescription;
        }
    }
}
