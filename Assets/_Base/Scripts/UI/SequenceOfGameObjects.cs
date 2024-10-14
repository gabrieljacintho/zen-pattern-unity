using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace FireRingStudio.UI
{
    public class SequenceOfGameObjects : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _sequenceOfGameObjects;
        [SerializeField] private InputActionReference _nextInput;

        [Space]
        public UnityEvent OnEnd;

        private int _index = -1;


        private void OnEnable()
        {
            if (_nextInput != null)
            {
                _nextInput.action.performed += Next;
            }
        }

        private void OnDisable()
        {
            if (_nextInput != null)
            {
                _nextInput.action.performed -= Next;
            }
        }

        public void Restart()
        {
            _index = 0;
            UpdateGameObject();
        }

        public void Next()
        {
            _index++;
            UpdateGameObject();
        }

        private void Next(InputAction.CallbackContext context) => Next();

        public void Disable()
        {
            if (_sequenceOfGameObjects == null)
            {
                return;
            }

            foreach (GameObject gameObject in _sequenceOfGameObjects)
            {
                gameObject.SetActive(false);
            }
        }

        public void UpdateGameObject()
        {
            if (_sequenceOfGameObjects == null || _index > _sequenceOfGameObjects.Count)
            {
                return;
            }

            if (_index == _sequenceOfGameObjects.Count)
            {
                OnEnd.Invoke();
                return;
            }

            for (int i = 0; i < _sequenceOfGameObjects.Count; i++)
            {
                _sequenceOfGameObjects[i].SetActive(i == _index);
            }
        }
    }
}