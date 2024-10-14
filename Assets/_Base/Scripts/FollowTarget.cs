using UnityEngine;

namespace FireRingStudio
{
    public class FollowTarget : Follow
    {
		[Space]
        [SerializeField] private Transform _target;
        [SerializeField] private string _targetObjectID;
        
        public Transform Target
        {
            get => GetTarget();
            set => _target = value;
        }
        public string TargetObjectID
        {
            get => _targetObjectID;
            set
            {
                if (_targetObjectID != value)
                {
                    _targetObjectID = value;
                    Target = null;
                }
            }
        }
        
        
        protected override Vector3 GetTargetPosition()
        {
            return Target != null ? Target.position : transform.position;
        }

        protected override Quaternion GetTargetRotation()
        {
            return Target != null ? Target.rotation : transform.rotation;
        }

        private Transform GetTarget()
        {
            if (_target != null)
            {
                return _target;
            }

            if (!string.IsNullOrEmpty(_targetObjectID))
            {
                GameObject targetObject = GameObjectID.FindGameObjectWithID(_targetObjectID);
                if (targetObject != null)
                {
                    _target = targetObject.transform;
                }
            }
            
            return _target;
        }
    }
}