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
        public float strength;
        public Element element;
        public CreateProjectile(int path, float strength, Element element)
        {
            this.path = path;
            this.strength = strength;
            this.element = element;
        }

        public override string GetDescription()
        {
            string pathDescription;
            string direction;
            switch (path)
            {
                case 0:
                    pathDescription = " along the chosen path.";
                    break;
                case 1:
                    direction = path < 0 ? "left" : "right";
                    pathDescription = " one path to the " + direction + ".";
                    break;
                case 2:
                    direction = path < 0 ? "left" : "right";
                    pathDescription = "two paths to the " + direction + ".";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Invalid path value in CreateProjectile.");
            }
            return "Creates a " + element.ToString() + " projectile of strength " + strength + pathDescription;
        }
    }
}
