using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace FireRingStudio
{
    public class DestroyGameObject : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;

        [Space]
        public UnityEvent OnDestroy;


        [Button]
        public void Destroy()
        {
            Destroy(_gameObject);
        }

        public void Destroy(GameObject gameObject)
        {
            Object.Destroy(gameObject);
            OnDestroy?.Invoke();
        }
    }
}