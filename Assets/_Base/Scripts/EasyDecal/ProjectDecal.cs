using ch.sycoforge.Decal;
using FireRingStudio.Cache;
using FireRingStudio.Physics;
using FireRingStudio.Pool;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace FireRingStudio.Surface
{
    public class ProjectDecal : MonoBehaviour
    {
        [SerializeField] private Caster _groundCaster;
        [SerializeField] private EasyDecal _prefab;
        [SerializeField] private float _minInterval;

        private float _previousProjectionTime;


        [Button]
        public void TryProject()
        {
            if (_minInterval > 0f && Time.time - _previousProjectionTime < _minInterval)
            {
                return;
            }

            if (_groundCaster == null || !_groundCaster.IsDetecting)
            {
                return;
            }

            if (!_groundCaster.TryGetClosestHit(out RaycastHit hit))
            {
                return;
            }

            try
            {
                GameObject prefab = _prefab.gameObject;
                EasyDecal.Project(prefab, hit.point, hit.normal, transform.eulerAngles.y);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message, this);
            }
            finally
            {
                _previousProjectionTime = Time.time;
            }
        }
    }
}