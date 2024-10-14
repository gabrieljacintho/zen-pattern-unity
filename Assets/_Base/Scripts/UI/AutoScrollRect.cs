using FireRingStudio.Cache;
using UnityEngine;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class AutoScrollRect : MonoBehaviour
    {
        [SerializeField] private float _smoothTime = 0.3f;
        
        private ScrollRect _scrollRect;

        private Vector2 _velocity;
        

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
        }

        private void LateUpdate()
        {
            GameObject selectedObject = EventManager.SelectedObject;
            if (selectedObject == null || !selectedObject.transform.IsChildOf(transform))
            {
                return;
            }

            RectTransform rectTransform = ComponentCacheManager.GetComponent<RectTransform>(selectedObject);
            
            SnapTo(rectTransform);
        }

        public void SnapTo(RectTransform target)
        {
            if (_scrollRect.content == null)
            {
                return;
            }

            Canvas.ForceUpdateCanvases();
            
            Transform scrollTransform = _scrollRect.transform;
            
            Vector2 contentPosition = scrollTransform.InverseTransformPoint(_scrollRect.content.position);
            Vector2 viewportPosition = scrollTransform.InverseTransformPoint(_scrollRect.viewport.transform.position);
            Vector2 targetPosition = scrollTransform.InverseTransformPoint(target.position);

            targetPosition = contentPosition - targetPosition - viewportPosition;
            targetPosition.x = 0f;

            if (targetPosition.y < 0f)
            {
                targetPosition.y = 0f;
            }

            float maxY = _scrollRect.content.rect.height - _scrollRect.viewport.rect.height;
            if (targetPosition.y > maxY)
            {
                targetPosition.y = maxY;
            }
            
            targetPosition = Vector2.SmoothDamp(_scrollRect.content.anchoredPosition, targetPosition, ref _velocity, _smoothTime, Mathf.Infinity, Time.unscaledDeltaTime);
            
            _scrollRect.content.anchoredPosition = targetPosition;
        }
    }
}