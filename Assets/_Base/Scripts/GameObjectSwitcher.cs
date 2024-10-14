using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace FireRingStudio
{
    public class GameObjectSwitcher : MonoBehaviour
    {
        [SerializeField] private List<KeyValue<string, GameObject>> _gameObjectsById;
        [SerializeField] private GameObject _defaultObject;

        private string _currentObjectId;

        public string CurrentObjectId => _currentObjectId;


        protected virtual void Awake()
        {
            ResetGameObject();
        }

        [Button]
        public virtual void SwitchGameObject(string id)
        {
            SwitchGameObject(gameObjectId => gameObjectId.Key == id);
            _currentObjectId = id;
        }

        public virtual void SwitchGameObject(GameObject gameObject)
        {
            SwitchGameObject(gameObjectId => gameObjectId.Value == gameObject);
            _currentObjectId = null;
        }

        public virtual void SwitchGameObject(Predicate<KeyValue<string, GameObject>> match)
        {
            bool activated = false;

            if (_gameObjectsById != null)
            {
                foreach (KeyValue<string, GameObject> gameObjectById in _gameObjectsById)
                {
                    GameObject value = gameObjectById.Value;
                    if (value == null)
                    {
                        continue;
                    }

                    bool matched = match.Invoke(gameObjectById);
                    value.SetActive(matched);
                    activated |= matched;
                }
            }

            if (!activated && _defaultObject != null)
            {
                _defaultObject.SetActive(true);
            }
        }

        public virtual void ResetGameObject()
        {
            if (_gameObjectsById != null)
            {
                foreach (KeyValue<string, GameObject> gameObjectById in _gameObjectsById)
                {
                    if (gameObjectById.Value != null)
                    {
                        gameObjectById.Value.SetActive(false);
                    }
                }
            }

            if (_defaultObject != null)
            {
                _defaultObject.SetActive(true);
            }

            _currentObjectId = null;
        }
    }
}