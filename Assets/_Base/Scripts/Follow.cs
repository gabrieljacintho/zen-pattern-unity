using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public abstract class Follow : MonoBehaviour
    {
        [SerializeField] private bool _canFollow = true;
        [SerializeField] private float _smoothTime = 0.3f;
        [SerializeField] private bool _unscaledDeltaTime;
        [SerializeField, Min(0f)] private float _reachTolerance = 0.1f;
        [SerializeField] private bool _canFollowRotation;
        
        private Vector3 _velocity;
        private bool _wasReach;

        public bool CanFollow
        {
            get => _canFollow;
            set => _canFollow = value;
        }
        public Vector3 TargetPosition => GetTargetPosition();
        public Quaternion TargetRotation => GetTargetRotation();
        public bool Reached => Vector3.Distance(transform.position, TargetPosition) <= _reachTolerance;
        
        [Space]
        public UnityEvent OnTargetReached;
        
        protected virtual void LateUpdate()
        {
            if (!CanFollow)
            {
                return;
            }

            FollowPosition();

            if (_canFollowRotation)
            {
                FollowRotation();
            }
            
            if (!_wasReach && Reached)
            {
                _wasReach = true;
                OnTargetReached?.Invoke();
            }
        }

        private void FollowPosition()
        {
            Vector3 position = TargetPosition;

            if (_smoothTime > 0f)
            {
                float deltaTime = _unscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
                position = Vector3.SmoothDamp(transform.position, position, ref _velocity, _smoothTime, Mathf.Infinity, deltaTime);
            }

            transform.position = position;
        }

        private void FollowRotation()
        {
            transform.rotation = TargetRotation;
        }

        public void Snap()
        {
            transform.position = TargetPosition;
        }
        
        protected abstract Vector3 GetTargetPosition();

        protected abstract Quaternion GetTargetRotation();
    }
}