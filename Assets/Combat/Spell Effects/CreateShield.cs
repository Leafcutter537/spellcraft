using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat.SpellEffects
{
    public class CreateShield : SpellEffect
    {
        public int path;
        public int strength;
        public Element element;
        public int duration;
        public CreateShield(int path, int strength, Element element, int duration)
        {
            this.path = path;
            this.strength = strength;
            this.element = element;
            this.duration = duration;
        }

        public override string GetDescription()
        {
            return GetDescription(null);
        }
        public override string GetDescription(StatBundle bundle)
        {
            string pathDescription;
            string direction;
            switch (Math.Abs(path))
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
                bonusString = " (+" + bundle.shieldPower + ")";
            }
            return "Creates a " + element.ToString() + " shield of strength " + strength + bonusString + pathDescription + " Lasts " + duration + " turns.";
        }
    }
}
