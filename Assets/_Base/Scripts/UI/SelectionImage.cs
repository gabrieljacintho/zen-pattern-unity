using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SelectionImage : MonoBehaviour
    {
        private RectTransform _rectTransform;
        
        private GameObject _currentSelectedObject;
        private RectTransform _currentSelectedRectTransform;

        private readonly Dictionary<GameObject, RectTransform> _rectTransforms = new Dictionary<GameObject, RectTransform>();
        

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            GameObject selectedObject = CanvasInput.SelectedObject;
            if (selectedObject == _currentSelectedObject)
            {
                UpdateTransform();
                return;
            }
            
            _currentSelectedObject = selectedObject;
            _currentSelectedRectTransform = selectedObject != null ? GetRectTransform(selectedObject) : null;
            
            if (selectedObject != null && IsSelectable(selectedObject))
            {
                gameObject.SetActiveChildren(true);
                UpdateTransform();
            }
            else
            {
                gameObject.SetActiveChildren(false);
            }
        }

        private void UpdateTransform()
        {
            if (_currentSelectedObject != null)
            {
                transform.position = _currentSelectedObject.transform.position;
            }

            if (_currentSelectedRectTransform != null)
            {
                _rectTransform.sizeDelta = _currentSelectedRectTransform.sizeDelta;
            }
        }

        private bool IsSelectable(GameObject targetObject)
        {
            RectTransform rectTransform = GetRectTransform(targetObject);
            if (rectTransform == null)
            {
                return false;
            }
            
            return transform.parent.ContainsChild(targetObject.transform, true);
        }

        private RectTransform GetRectTransform(GameObject targetObject)
        {
            if (_rectTransforms.TryGetValue(targetObject, out RectTransform rectTransform))
            {
                return rectTransform;
            }
            
            rectTransform = targetObject.GetComponent<RectTransform>();
            _rectTransforms.Add(targetObject, rectTransform);

            return rectTransform;
        }
    }
}