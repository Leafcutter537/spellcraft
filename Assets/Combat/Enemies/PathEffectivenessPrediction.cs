using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Combat
{
    public class PathEffectivenessPrediction
    {
        public Path path;
        public int predictedEffectiveness;

        public PathEffectivenessPrediction(Path path, int predictedEffectiveness)
        {
            this.path = path;
            this.predictedEffectiveness = predictedEffectiveness;
        }
    }
}
