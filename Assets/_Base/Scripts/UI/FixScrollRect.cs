using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    public class FixScrollRect: MonoBehaviour, IBeginDragHandler,  IDragHandler, IEndDragHandler, IScrollHandler
    {
        [SerializeField] private ScrollRect _scrollRect;
        [SerializeField] private bool _getInParent = true;


        private void Awake()
        {
            if (_getInParent && _scrollRect == null)
            {
                _scrollRect = GetComponentInParent<ScrollRect>();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnBeginDrag(eventData);
            }
        }
 
        public void OnDrag(PointerEventData eventData)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnDrag(eventData);
            }
        }
 
        public void OnEndDrag(PointerEventData eventData)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnEndDrag(eventData);
            }
        }
 
        public void OnScroll(PointerEventData data)
        {
            if (_scrollRect != null)
            {
                _scrollRect.OnScroll(data);
            }
        }
    }
}