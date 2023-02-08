using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class PathDamagePrediction
    {
        public Path path;
        public int predictedDamage;

        public PathDamagePrediction(Path path, int predictedDamage)
        {
            this.path = path;
            this.predictedDamage = predictedDamage;
        }
    }
}
