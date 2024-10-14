#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomDrawer&lt;DamageEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DamageEvent))]
    public class DamageEventDrawer : AtomDrawer<DamageEvent> { }
}
#endif
