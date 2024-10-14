using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.Inventory.UI
{
    public class ItemPackDisplay : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TextMeshProUGUI _sizeText;
        [SerializeField] private bool _hideIfEmpty = true;

        private ItemPack _itemPack;
        private ItemDraggableGraphic _draggableGraphic;
        
        private bool _subscribed;

        public ItemPack ItemPack => _itemPack;
        private ItemData ItemData => _itemPack?.Data;


        private void OnEnable()
        {
            if (_itemPack != null && !_subscribed)
            {
                _itemPack.Changed += UpdatePanel;
                _subscribed = true;
            }

            UpdatePanel();
        }

        private void OnDisable()
        {
            if (_itemPack != null)
            {
                _itemPack.Changed -= UpdatePanel;
                _subscribed = false;
            }
        }

        public void Initialize(ItemPack itemPack)
        {
            if (_itemPack == itemPack)
            {
                return;
            }
            
            if (_itemPack != null && _subscribed)
            {
                _itemPack.Changed -= UpdatePanel;
            }
            
            _itemPack = itemPack;
            if (_itemPack != null)
            {
                _itemPack.Changed += UpdatePanel;
                _subscribed = true;
            }
            
            UpdatePanel();
        }

        private void UpdatePanel()
        {
            UpdateIconImage();
            UpdateSizeText();
        }
        
        private void UpdateIconImage()
        {
            if (_iconImage == null)
            {
                return;
            }

            _iconImage.sprite = ItemData != null && CanShow() ? ItemData.Icon : null;
            _iconImage.gameObject.SetActive(_iconImage.sprite != null);
        }

        private void UpdateSizeText()
        {
            if (_sizeText == null)
            {
                return;
            }

            if (_itemPack != null && _itemPack.MaxSize != 1 && CanShow())
            {
                _sizeText.text = _itemPack.Size.ToString();
                _sizeText.gameObject.SetActive(true);
            }
            else
            {
                _sizeText.gameObject.SetActive(false);
            }
        }

        private bool CanShow()
        {
            return ItemPack != null && (!_hideIfEmpty || !ItemPack.IsEmpty);
        }
    }
}