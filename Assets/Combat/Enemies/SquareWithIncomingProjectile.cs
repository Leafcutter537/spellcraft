using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class SquareWithIncomingProjectile : IComparable<SquareWithIncomingProjectile>
    {
        public int incomingProjectileStrength;
        public GridSquare square;

        public SquareWithIncomingProjectile(int incomingProjectileStrength, GridSquare square)
        {
            this.incomingProjectileStrength = incomingProjectileStrength;
            this.square = square;
        }

        public int CompareTo(SquareWithIncomingProjectile other)
        {
            return incomingProjectileStrength.CompareTo(other.incomingProjectileStrength);
        }
    }
}
