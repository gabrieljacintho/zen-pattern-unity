#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityEngine.UIElements;
using UnityAtoms.Editor;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DamagePair`. Inherits from `AtomEventEditor&lt;DamagePair, DamagePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomEditor(typeof(DamagePairEvent))]
    public sealed class DamagePairEventEditor : AtomEventEditor<DamagePair, DamagePairEvent> { }
}
#endif
