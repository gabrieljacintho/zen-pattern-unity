using UnityEngine;
using UnityEngine.InputSystem;

namespace FireRingStudio
{
    public class FreeMovement : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveInput;
        [SerializeField] private InputActionReference _moveUpInput;
        [SerializeField] private InputActionReference _moveDownInput;
        [SerializeField] private InputActionReference _allowMoveInput;
        
        [Header("Settings")]
        [SerializeField] private float _maxSpeed = 1f;
        [SerializeField, Min(0f)] private float _accelerationSpeed = 10f;
        
        private float _currentSpeed;

        
        private void Update()
        {
            if (_allowMoveInput != null && !_allowMoveInput.action.IsPressed())
            {
                return;
            }
            
            Vector2 move = _moveInput != null ? _moveInput.action.ReadValue<Vector2>() : Vector2.zero;
            bool moveUp = _moveUpInput != null && _moveUpInput.action.IsPressed();
            bool moveDown = _moveDownInput != null && _moveDownInput.action.IsPressed();

            UpdateCurrentSpeed(move.magnitude > 0f || moveUp || moveDown);

            Vector3 position = transform.position;
            float speed = _currentSpeed * Time.unscaledDeltaTime;
            
            if (move.magnitude > 0f)
            {
                position += transform.forward * (move.y * speed);
                position += transform.right * (move.x * speed);
            }

            if (moveUp)
            {
                position += transform.up * speed;
            }
            
            if (moveDown)
            {
                position += -transform.up * speed;
            }
            
            transform.position = position;
        }

        private void UpdateCurrentSpeed(bool canMove)
        {
            if (canMove)
            {
                _currentSpeed = Mathf.MoveTowards(_currentSpeed, _maxSpeed, _accelerationSpeed * Time.unscaledDeltaTime);
            }
            else
            {
                _currentSpeed = 0f;
            }
        }
    }
}