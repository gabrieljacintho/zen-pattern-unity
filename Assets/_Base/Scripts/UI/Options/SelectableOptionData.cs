using I2.Loc;
using UnityEngine;

namespace FireRingStudio.UI.Options
{
    [CreateAssetMenu(menuName = "FireRing Studio/Selectable Option Data", fileName = "New Selectable Option Data")]
    public class SelectableOptionData : ScriptableObject
    {
        public LocalizedString DisplayName;
        public Sprite Icon;
        public int Order;
    }
}