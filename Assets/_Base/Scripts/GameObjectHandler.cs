using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    public class GameObjectHandler : MonoBehaviour
    {
        [Tooltip("If null the component GameObject is used.")]
        [SerializeField] private GameObject _target;

        [Space]
        [SerializeField] private bool _enableOnGameStart;
        [HideIf("_enableOnGameStart")]
        [SerializeField] private bool _disableOnGameStart;
        
        [Space]
        [FormerlySerializedAs("_enableOnGameEnd")]
        [SerializeField] private bool _enableOnGameEndOrReset;
        [FormerlySerializedAs("_disableOnGameEnd")]
        [HideIf("_enableOnGameEndOrReset")]
        [SerializeField] private bool _disableOnGameEndOrReset;

        public GameObject Target
        {
            get => _target != null ? _target : gameObject;
            set => _target = value;
        }


        private void Awake()
        {
            GameManager.GameStarted += OnGameStart;
            GameManager.GameOver += OnGameEndOrReset;
            GameManager.GameReset += OnGameEndOrReset;
        }

        private void OnDestroy()
        {
            GameManager.GameStarted -= OnGameStart;
            GameManager.GameOver -= OnGameEndOrReset;
            GameManager.GameReset -= OnGameEndOrReset;
        }

        public void SetActiveTarget(bool value)
        {
            Target.SetActive(value);
        }

        public void EnableTarget() => SetActiveTarget(true);

        public void DisableTarget() => SetActiveTarget(false);

        public void DestroyTarget()
        {
            Destroy(Target);
        }

        public void SetActiveTargetChildren(bool value)
        {
            Target.SetActiveChildren(value);
        }

        public void EnableTargetChildren() => SetActiveTargetChildren(true);
        public void DisableTargetChildren() => SetActiveTargetChildren(false);

        private void OnGameStart()
        {
            if (_enableOnGameStart)
            {
                EnableTarget();
            }
            else if (_disableOnGameStart)
            {
                DisableTarget();
            }
        }

        private void OnGameEndOrReset()
        {
            if (_enableOnGameEndOrReset)
            {
                EnableTarget();
            }
            else if (_disableOnGameEndOrReset)
            {
                DisableTarget();
            }
        }
    }
}