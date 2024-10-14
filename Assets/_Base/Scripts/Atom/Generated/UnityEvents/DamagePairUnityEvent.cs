using System;
using UnityEngine.Events;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms
{
    /// <summary>
    /// None generic Unity Event of type `DamagePair`. Inherits from `UnityEvent&lt;DamagePair&gt;`.
    /// </summary>
    [Serializable]
    public sealed class DamagePairUnityEvent : UnityEvent<DamagePair> { }
}
