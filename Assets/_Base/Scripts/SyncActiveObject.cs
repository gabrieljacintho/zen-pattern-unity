using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio
{
    public class SyncActiveObject : MonoBehaviour
    {
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private List<GameObject> _otherObjects;


        private void LateUpdate()
        {
            if (_otherObjects == null)
            {
                return;
            }
            
            bool isActive = _targetObject != null && _targetObject.activeSelf;

            _otherObjects.ForEach(otherObject => otherObject.SetActive(isActive));
        }
    }
}