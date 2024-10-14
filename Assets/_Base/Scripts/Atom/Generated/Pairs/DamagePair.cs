using System;
using UnityEngine;
using FireRingStudio.Vitals;
namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// IPair of type `&lt;FireRingStudio.Vitals.Damage&gt;`. Inherits from `IPair&lt;FireRingStudio.Vitals.Damage&gt;`.
    /// </summary>
    [Serializable]
    public struct DamagePair : IPair<FireRingStudio.Vitals.Damage>
    {
        public FireRingStudio.Vitals.Damage Item1 { get => _item1; set => _item1 = value; }
        public FireRingStudio.Vitals.Damage Item2 { get => _item2; set => _item2 = value; }

        [SerializeField]
        private FireRingStudio.Vitals.Damage _item1;
        [SerializeField]
        private FireRingStudio.Vitals.Damage _item2;

        public void Deconstruct(out FireRingStudio.Vitals.Damage item1, out FireRingStudio.Vitals.Damage item2) { item1 = Item1; item2 = Item2; }
    }
}