using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace FireRingStudio.Extensions
{
    public static class ComponentExtensions
    {
        public static void SetSource(this ParentConstraint parentConstraint, Transform source, float weight = 1f)
        {
            ConstraintSource constraintSource = new ConstraintSource
            {
                sourceTransform = source,
                weight = weight
            };

            parentConstraint.SetSources(new List<ConstraintSource> { constraintSource });
        }
    }
}