#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable property drawer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomDrawer&lt;DamageVariable&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DamageVariable))]
    public class DamageVariableDrawer : VariableDrawer<DamageVariable> { }
}
#endif
