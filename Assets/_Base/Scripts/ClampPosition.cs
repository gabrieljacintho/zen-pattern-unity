using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio
{
    public class ClampPosition : MonoBehaviour
    {
        [SerializeField] private float _minYPosition;


        [Button]
        public void Clamp()
        {
            Vector3 position = transform.position;

            if (position.y < _minYPosition)
            {
                position.y = _minYPosition;
            }

            transform.position = position;
        }
    }
}