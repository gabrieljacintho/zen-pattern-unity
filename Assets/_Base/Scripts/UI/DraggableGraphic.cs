using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(Graphic))]
    public class DraggableGraphic : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private bool _canDrag = true;
        [SerializeField] private InputActionReference _mousePositionInput;
        [SerializeField] private FollowTarget _follow;
        
        private Graphic _graphic;
        private Transform _defaultParent;
        private int _defaultSiblingIndex = -1;
        private Transform _root;
        
        private bool _isDragging;
        
        public Graphic Graphic
        {
            get
            {
                if (_graphic == null)
                {
                    _graphic = GetComponent<Graphic>();
                }

                return _graphic;
            }
        }
        public Transform DefaultParent
        {
            get
            {
                if (_defaultParent == null)
                {
                    _defaultParent = transform.parent;
                }

                return _defaultParent;
            }
            set => _defaultParent = value;
        }
        public int DefaultSiblingIndex
        {
            get
            {
                if (_defaultSiblingIndex == -1)
                {
                    _defaultSiblingIndex = transform.GetSiblingIndex();
                }

                return _defaultSiblingIndex;
            }
            set => _defaultSiblingIndex = value;
        }
        private Transform Root
        {
            get
            {
                if (_root == null)
                {
                    Canvas canvas = GetComponentInParent<Canvas>();
                    _root = canvas != null ? canvas.transform : transform.root;
                }

                return _root;
            }
        }
        
        public bool IsDragging => _isDragging;
        public virtual bool CanDrag => _canDrag && (IsDragging || Reached);
        protected virtual bool CanAutoAttach => !IsDragging && Reached;
        private bool Reached => _follow == null || _follow.Reached;
        
        [Space]
        public UnityEvent OnDragBegan;
        public UnityEvent OnDragEnded;


        protected virtual void LateUpdate()
        {
            if (!IsDragging)
            {
                if (CanAutoAttach)
                {
                    Attach();
                }
                else
                {
                    Detach();
                }
            }
            
            UpdateFollow();
        }

        protected virtual void OnDisable()
        {
            if (IsDragging)
            {
                Attach();
                _isDragging = false;
            }

            if (_follow != null)
            {
                UpdateFollow();
                _follow.Snap();
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!CanDrag)
            {
                return;
            }
            
            DefaultParent = transform.parent;
            DefaultSiblingIndex = transform.GetSiblingIndex();

            Detach();
            
            _isDragging = true;
            
            OnDragBegan?.Invoke();
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!CanDrag || _mousePositionInput == null)
            {
                return;
            }
            
            transform.position = _mousePositionInput.action.ReadValue<Vector2>();
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!CanDrag && !IsDragging)
            {
                return;
            }

            Attach();
            
            _isDragging = false;
            
            OnDragEnded?.Invoke();
        }

        public virtual void Detach()
        {
            transform.SetParent(Root);
            transform.SetAsLastSibling();
            Graphic.raycastTarget = false;
        }

        public virtual void Attach()
        {
            transform.SetParent(DefaultParent);
            transform.SetSiblingIndex(DefaultSiblingIndex);
            Graphic.raycastTarget = true;
        }

        private void UpdateFollow()
        {
            if (_follow == null)
            {
                return;
            }
            
            _follow.Target = DefaultParent != null ? DefaultParent : transform;
            _follow.CanFollow = !IsDragging;
        }
    }
}