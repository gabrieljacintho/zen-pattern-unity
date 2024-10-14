using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    [RequireComponent(typeof(ReflectionProbe))]
    public class ReflectionProbeUpdater : MonoBehaviour
    {
        [SerializeField] private bool _canUpdate = true;
        [SerializeField] private float _updateDelay = 10f;
        [SerializeField] private float _updateDistance = 10f;
        [SerializeField] private bool _useUnscaledDeltaTime;

        private ReflectionProbe _reflectionProbe;

        private float _time;
        private Vector3 _lastPosition;

        public bool CanUpdate
        {
            get => _canUpdate;
            set => _canUpdate = value;
        }


        private void Awake()
        {
            _reflectionProbe = GetComponent<ReflectionProbe>();
            ResetUpdater();
        }

        private void Update()
        {
            if (!_canUpdate || !_reflectionProbe.enabled)
            {
                ResetUpdater();
                return;
            }

            if (_time >= _updateDelay)
            {
                RenderProbe();
            }
            else if (Vector3.Distance(transform.position, _lastPosition) >= _updateDistance)
            {
                RenderProbe();
            }

            if (_useUnscaledDeltaTime)
            {
                _time += Time.unscaledDeltaTime;
            }
            else
            {
                _time += Time.deltaTime;
            }
        }

        [Button]
        public void RenderProbe()
        {
            _reflectionProbe.RenderProbe();
            ResetUpdater();
        }

        public void ResetUpdater()
        {
            _time = 0f;
            _lastPosition = transform.position;
        }
    }
}