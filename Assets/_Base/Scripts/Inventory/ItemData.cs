using I2.Loc;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.Inventory
{
    [CreateAssetMenu(menuName = "FireRing Studio/Inventory/Item Data", fileName = "New Item Data")]
    public class ItemData : ScriptableObject
    {
        public string Id;
        
        [Space]
        [FormerlySerializedAs("category")]
        public StringConstant Category;
        public LocalizedString DisplayName;
        public LocalizedString Description;
        [FormerlySerializedAs("icon")]
        [PreviewField] public Sprite Icon;
        [FormerlySerializedAs("prefab")]
        [PreviewField] public GameObject Prefab;
        [Tooltip("Set negative to not limit.")]
        public IntReference MaxPackSizeReference = new IntReference(-1);
        
        [Header("Options")]
        public bool CanBeMoved = true;
        public bool CanBeSelected = true;
        public bool CanBeDiscarded = true;

        public int MaxPackSize => MaxPackSizeReference?.Value ?? 0;
    }
}
