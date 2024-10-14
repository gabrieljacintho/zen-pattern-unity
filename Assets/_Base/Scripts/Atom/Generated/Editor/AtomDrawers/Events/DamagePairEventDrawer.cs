#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Event property drawer of type `DamagePair`. Inherits from `AtomDrawer&lt;DamagePairEvent&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DamagePairEvent))]
    public class DamagePairEventDrawer : AtomDrawer<DamagePairEvent> { }
}
#endif
