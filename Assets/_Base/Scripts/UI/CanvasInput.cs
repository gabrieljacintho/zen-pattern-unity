using System.Collections.Generic;
using FireRingStudio.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Canvas))]
    public class CanvasInput : MonoBehaviour
    {
        [SerializeField] private GameObject _firstSelectedObject;

        private Canvas _canvas;
        
        private readonly Dictionary<GameObject, Selectable> _selectables = new Dictionary<GameObject, Selectable>();
        private readonly LimitedQueue<GameObject> _selectionHistory = new LimitedQueue<GameObject>(8);

        private static readonly List<CanvasInput> s_activeCanvasInputs = new List<CanvasInput>();
        
        public static GameObject SelectedObject
        {
            get
            {
                CanvasInput value = GetCanvasOnTop();
                if (value != null)
                {
                    return value.LocalSelectedObject;
                }
                
                return EventManager.SelectedObject;
            }
        }
        
        private GameObject LocalSelectedObject
        {
            get => _selectionHistory[0];
            set => SetSelectedObject(value);
        }
        
        [Space]
        [FormerlySerializedAs("onSelectedObjectChanged")]
        public UnityEvent<GameObject> OnSelectedObjectChanged;


        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            if (!s_activeCanvasInputs.Contains(this))
            {
                s_activeCanvasInputs.Add(this);
            }

            this.DoOnNextFrame(() =>
            {
                LocalSelectedObject = _firstSelectedObject;
            });
        }

        private void Update()
        {
            if (GetCanvasOnTop() != this)
            {
                return;
            }
            
            UpdateSelectedObject();
        }

        private void OnDisable()
        {
            s_activeCanvasInputs.Remove(this);
        }
        
        private void UpdateSelectedObject()
        {
            if (EventManager.EventSystem == null)
            {
                return;
            }

            GameObject selectedObject = EventManager.SelectedObject;

            if (selectedObject != null && IsSelectable(selectedObject))
            {
                SetSelectedObject(selectedObject);
            }
            else if (LocalSelectedObject != null)
            {
                if (IsSelectable(LocalSelectedObject))
                {
                    EventManager.SelectedObject = LocalSelectedObject;
                }
                else
                {
                    SetSelectedObject(GetPreviousSelectableObject());
                }
            }
            else if (_firstSelectedObject != null && IsSelectable(_firstSelectedObject))
            {
                LocalSelectedObject = _firstSelectedObject;
            }
        }

        private void SetSelectedObject(GameObject value)
        {
            if (LocalSelectedObject == value || EventManager.EventSystem == null)
            {
                return;
            }

            EventManager.SelectedObject = value;
            _selectionHistory.Enqueue(value);

            OnSelectedObjectChanged?.Invoke(value);
        }
        
        private GameObject GetPreviousSelectableObject()
        {
            for (int i = 1; i < _selectionHistory.Length; i++)
            {
                GameObject selectionObject = _selectionHistory[i];
                if (selectionObject != null && IsSelectable(selectionObject))
                {
                    return selectionObject;
                }
            }

            return null;
        }

        public bool IsSelectable(GameObject targetObject)
        {
            if (!targetObject.activeInHierarchy)
            {
                return false;
            }

            List<Transform> children = transform.GetChildrenRecursively();
            if (!children.Contains(targetObject.transform))
            {
                return false;
            }
            
            if (!_selectables.TryGetValue(targetObject, out Selectable selectable))
            {
                selectable = targetObject.GetComponent<Selectable>();
                _selectables.Add(targetObject, selectable);
            }

            if (selectable == null || !selectable.interactable)
            {
                return false;
            }

            return true;
        }

        private static CanvasInput GetCanvasOnTop()
        {
            CanvasInput value = null;
            foreach (CanvasInput canvasInput in s_activeCanvasInputs)
            {
                if (value == null || canvasInput._canvas.sortingOrder > value._canvas.sortingOrder)
                {
                    value = canvasInput;
                }
            }
            
            return value;
        }
    }
}