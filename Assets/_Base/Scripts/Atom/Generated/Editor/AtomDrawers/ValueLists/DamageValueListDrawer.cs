#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Value List property drawer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomDrawer&lt;DamageValueList&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DamageValueList))]
    public class DamageValueListDrawer : AtomDrawer<DamageValueList> { }
}
#endif
