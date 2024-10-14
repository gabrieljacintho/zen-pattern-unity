using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio.Interaction
{
    [Serializable]
    public enum Axis
    {
        X,
        Y,
        Z
    }

    public class OpenableObject : InteractableBase
    {
        [Serializable]
        public enum OpeningConstraints
        {
            Free,
            OnlyToInside,
            OnlyToOutside
        }

        [SerializeField, ES3NonSerializable] private Lock _lock;
        [SerializeField, ES3NonSerializable] private List<Animator> _animators;
        [SerializeField, Range(-1f, 1f)] private float _openAmount;
        [SerializeField] private float _openDelay;
        [SerializeField, ES3NonSerializable] private float _smoothTime = 0.3f;
        [SerializeField, ES3NonSerializable] private Axis _outsideAxis = Axis.Z;
        [SerializeField, ES3NonSerializable] private bool _invertSides;
        [SerializeField, ES3NonSerializable] private OpeningConstraints _constraints;

        private float _initialOpenAmount;
        [ES3Serializable] private float _targetOpenAmount;
        private float _velocity;
        private float _openTime;
        
        private static readonly int s_openAmountProperty = Animator.StringToHash("Open Amount");

        public override bool Interactable => base.Interactable && IsUnlocked && !IsInTransition;
        public Lock Lock => _lock;
        public bool IsUnlocked => _lock == null || _lock.IsUnlocked;
        public bool IsOpen => IsUnlocked && Mathf.Abs(_openAmount) > 0.1f;
        public float OpenAmount
        {
            get => _openAmount;
            private set => SetOpenAmount(value);
        }
        private bool IsInTransition => Math.Abs(_openAmount - _targetOpenAmount) > 0.1f;
        
        [Space]
        [ES3NonSerializable] public UnityEvent OnOpen;
        [ES3NonSerializable] public UnityEvent OnClose;


        protected override void Awake()
        {
            base.Awake();

            if (_lock != null)
            {
                _lock.OpenableObject = this;

                if (!_lock.IsUnlocked)
                {
                    _openAmount = 0f;
                }
            }

            _initialOpenAmount = _openAmount;
            _targetOpenAmount = _openAmount;

            if (_openAmount != 0f)
            {
                OnOpen?.Invoke();
            }
            else
            {
                OnClose?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (GameManager.IsPaused)
            {
                return;
            }

            if (Mathf.Abs(_targetOpenAmount) > 0.1f)
            {
                if (_openTime < _openDelay)
                {
                    _openTime += Time.deltaTime;
                    return;
                }
            }

            OpenAmount = Mathf.SmoothDamp(_openAmount, _targetOpenAmount, ref _velocity, _smoothTime);
        }

        public virtual bool TryOpen(Vector3 agentPosition)
        {
            if (!Interactable)
            {
                return false;
            }

            ForceOpen(agentPosition);

            return true;
        }

        public virtual void ForceOpen(Vector3 agentPosition)
        {
            bool openToOutside;

            switch (_constraints)
            {
                case OpeningConstraints.Free:
                    openToOutside = IsOutside(agentPosition);
                    break;

                case OpeningConstraints.OnlyToInside:
                    openToOutside = false;
                    break;

                case OpeningConstraints.OnlyToOutside:
                    openToOutside = true;
                    break;

                default:
                    openToOutside = false;
                    break;
            }

            ForceOpen(openToOutside);
        }

        private bool IsOutside(Vector3 agentPosition)
        {
            agentPosition = transform.InverseTransformPoint(agentPosition);

            switch (_outsideAxis)
            {
                case Axis.X:
                    return agentPosition.x >= 0f;

                case Axis.Y:
                    return agentPosition.y >= 0f;

                case Axis.Z:
                    return agentPosition.z >= 0f;

                default:
                    return false;
            }
        }

        public virtual void ForceOpen(bool outside)
        {
            float openAmount = outside ? -1f : 1f;

            if (_invertSides)
            {
                openAmount *= -1f;
            }

            _targetOpenAmount = openAmount;
            _openTime = 0f;

            OnOpen?.Invoke();
        }

        public virtual bool TryClose()
        {
            if (!Interactable)
            {
                return false;
            }
            
            ForceClose();

            return true;
        }

        public virtual void ForceClose()
        {
            _targetOpenAmount = 0f;

            OnClose?.Invoke();
        }

        public void Restore()
        {
            OpenAmount = _initialOpenAmount;
        }

        private void SetOpenAmount(float value)
        {
            _openAmount = value;
            _animators?.ForEach(animator => animator.SetFloat(s_openAmountProperty, _openAmount));
        }
    }
}