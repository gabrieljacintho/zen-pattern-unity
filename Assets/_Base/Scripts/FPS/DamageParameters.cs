using System;
using FireRingStudio.Vitals;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.FPS
{
    [Serializable]
    public struct DamageParameters
    {
        public DamageType Type;
        public FloatReference MinAmountReference;
        public FloatReference MaxAmountReference;
        public FloatReference DurationReference;
        public LayerMask LayerMask;
        public QueryTriggerInteraction TriggerInteraction;

        public float MinAmount => MinAmountReference?.Value ?? 0f;
        public float MaxAmount => MaxAmountReference?.Value ?? 0f;
        public float Duration => DurationReference?.Value ?? 0f;
        
        [Header("Critical")]
        public FloatReference CriticalScaleReference;
        public FloatReference CriticalChanceReference;

        public float CriticalScale => CriticalScaleReference?.Value ?? 0f;
        public float CriticalChance => CriticalChanceReference?.Value ?? 0f;


        public float GetRandomDamageAmount()
        {
            float value = UnityEngine.Random.Range(MinAmount, MaxAmount);
            
            if (CriticalChance > 0f && UnityEngine.Random.Range(0f, 1f) <= CriticalChance)
            {
                value *= CriticalScale;
            }

            return value;
        }
    }
}