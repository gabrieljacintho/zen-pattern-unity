using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ButtonComponent : MonoBehaviour, IButton
    {
        [SerializeField] private Button _button;

        public Button Button
        {
            get
            {
                if (_button == null)
                {
                    _button = GetComponent<Button>();
                }

                return _button;
            }
        }


        protected virtual void OnEnable()
        {
            Button.onClick.AddListener(OnClick);
        }
        
        protected virtual void OnDisable()
        {
            Button.onClick.RemoveListener(OnClick);
        }

        protected abstract void OnClick();
    }
}