#if UNITY_2019_1_OR_NEWER
using UnityEditor;
using UnityAtoms.Editor;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Constant property drawer of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomDrawer&lt;DamageConstant&gt;`. Only availble in `UNITY_2019_1_OR_NEWER`.
    /// </summary>
    [CustomPropertyDrawer(typeof(DamageConstant))]
    public class DamageConstantDrawer : VariableDrawer<DamageConstant> { }
}
#endif
