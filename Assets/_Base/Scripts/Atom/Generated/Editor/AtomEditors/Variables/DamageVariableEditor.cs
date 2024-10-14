using UnityEditor;
using UnityAtoms.Editor;
using FireRingStudio.Vitals;

namespace UnityAtoms.BaseAtoms.Editor
{
    /// <summary>
    /// Variable Inspector of type `FireRingStudio.Vitals.Damage`. Inherits from `AtomVariableEditor`
    /// </summary>
    [CustomEditor(typeof(DamageVariable))]
    public sealed class DamageVariableEditor : AtomVariableEditor<FireRingStudio.Vitals.Damage, DamagePair> { }
}
