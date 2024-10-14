using RootMotion.FinalIK;
using UnityEngine;

namespace FireRingStudio.AI
{
    [RequireComponent(typeof(Animator))]
    public class LookAtIKHandler : MonoBehaviour
    {
        [SerializeField] private string _targetID;
        [SerializeField, Range(0f, 1f)] private float _weight = 1f;

        private LookAtIK _lookAt;

        
        private void Awake()
        {
            _lookAt = GetComponent<LookAtIK>();
        }

        private void OnEnable()
        {
            GameObjectID.Updated += UpdateTarget;
            UpdateTarget();
        }
        
        private void OnAnimatorIK(int layerIndex)
        {
            if (!isActiveAndEnabled || _lookAt == null)
            {
                return;
            }

            Transform target = _lookAt.solver.target;
            if (target == null || !target.gameObject.activeInHierarchy)
            {
                _lookAt.solver.SetLookAtWeight(0f);
                return;
            }
            
            _lookAt.solver.SetLookAtWeight(_weight);
        }

        private void OnDisable()
        {
            GameObjectID.Updated -= UpdateTarget;
        }

        private void UpdateTarget()
        {
            if (_lookAt == null || _lookAt.solver.target != null)
            {
                return;
            }

            GameObject targetObject = GameObjectID.FindGameObjectWithID(_targetID);
            if (targetObject == null)
            {
                return;
            }

            _lookAt.solver.target = targetObject.transform;
        }
    }
}