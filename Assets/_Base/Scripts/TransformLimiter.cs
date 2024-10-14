using UnityEngine;

namespace FireRingStudio
{
    public class TransformLimiter : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] private bool _minXPositionEnabled;
        [SerializeField] private float _minXPosition;
        [SerializeField] private bool _maxXPositionEnabled;
        [SerializeField] private float _maxXPosition;
        
        [Space]
        [SerializeField] private bool _minYPositionEnabled;
        [SerializeField] private float _minYPosition;
        [SerializeField] private bool _maxYPositionEnabled;
        [SerializeField] private float _maxYPosition;
        
        [Space]
        [SerializeField] private bool _minZPositionEnabled;
        [SerializeField] private float _minZPosition;
        [SerializeField] private bool _maxZPositionEnabled;
        [SerializeField] private float _maxZPosition;

        [Space]
        [SerializeField] private Space _space;

        // TODO: Rotation and Scale


        private void LateUpdate()
        {
            LimitPosition();
        }

        private void LimitPosition()
        {
            LimitXPosition();
            LimitYPosition();
            LimitZPosition();
        }

        private void LimitXPosition()
        {
            if (_minXPositionEnabled)
            {
                CheckMinXPosition();
            }

            if (_maxXPositionEnabled)
            {
                CheckMaxXPosition();
            }
        }

        private void CheckMinXPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.x >= _minXPosition)
            {
                return;
            }
            
            position.x = _minXPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }

        private void CheckMaxXPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.x <= _maxXPosition)
            {
                return;
            }

            position.x = _maxXPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }

        private void LimitYPosition()
        {
            if (_minYPositionEnabled)
            {
                CheckMinYPosition();
            }

            if (_maxYPositionEnabled)
            {
                CheckMaxYPosition();
            }
        }

        private void CheckMinYPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.y >= _minYPosition)
            {
                return;
            }
            
            position.y = _minYPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }

        private void CheckMaxYPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.y <= _maxYPosition)
            {
                return;
            }

            position.y = _maxYPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }
        
        private void LimitZPosition()
        {
            if (_minZPositionEnabled)
            {
                CheckMinZPosition();
            }

            if (_maxZPositionEnabled)
            {
                CheckMaxZPosition();
            }
        }

        private void CheckMinZPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.z >= _minZPosition)
            {
                return;
            }
            
            position.z = _minZPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }

        private void CheckMaxZPosition()
        {
            Vector3 position = _space == Space.Self ? transform.localPosition : transform.position;
            if (position.z <= _maxZPosition)
            {
                return;
            }

            position.z = _maxZPosition;
            if (_space == Space.Self)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
        }
    }
}