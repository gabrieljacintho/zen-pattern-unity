using FireRingStudio.Extensions;
using FireRingStudio.Helpers;
using UnityEngine;
using UnityEngine.AI;

namespace FireRingStudio.AI
{
    public class NavMeshPositioner : MonoBehaviour
    {
        [Tooltip("If null the component Parent or Transform is used.")]
        [SerializeField] private Transform _sourcePoint;
        [SerializeField] private Vector2 _distanceRange = Vector2.up * 4f;
        [SerializeField] private string _areaName = "Walkable";
        [SerializeField, Min(0f)] private float _heightTolerance = 0.2f;
        [SerializeField] private Vector3 _offset;
        [SerializeField, Min(0)] private int _updateInterval = 1;

        private NavMeshQueryFilter _filter;

        private Vector3 SourcePosition
        {
            get
            {
                if (_sourcePoint != null)
                {
                    return _sourcePoint.position;
                }

                return transform.parent != null ? transform.parent.position : transform.position;
            }
        }
        

        private void Awake()
        {
            NavMeshAgent agent = GetComponentInParent<NavMeshAgent>();
            if (agent != null)
            {
                _filter = agent.GetNavMeshQueryFilter();
            }
            else
            {
                _filter = new NavMeshQueryFilter
                {
                    areaMask = NavMesh.GetAreaFromName(_areaName)
                };
            }
        }

        private void Update()
        {
            if (_updateInterval == 0 || Time.frameCount % _updateInterval != 0)
            {
                return;
            }
            
            UpdatePosition();
        }

        public void UpdatePosition()
        {
            if (NavMeshHelper.NavMeshPosition(SourcePosition, out NavMeshHit hit, _distanceRange, _filter, _heightTolerance))
            {
                transform.position = hit.position + _offset;
            }
        }
    }
}