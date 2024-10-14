#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomEventEditor&lt;FireRingStudio.Vitals.Damage, DamageEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DamageEvent))]
    public sealed class DamageEventEditor : AtomEventEditor<FireRingStudio.Vitals.Damage, DamageEvent> { }
}
#endif
