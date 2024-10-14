using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio.Pool
{
    public class PooledObject : MonoBehaviour
    {
        #region Variables

        [SerializeField] private ObjectPool _pool;
        [HideIf("@_pool != null")]
        [SerializeField] private string _poolTag;
        
        [Space]
        [SerializeField] private bool _autoReleaseToPool;
        [ShowIf("_autoReleaseToPool")]
        [SerializeField] private float _autoReleaseDelay = 30f;
        [ShowIf("_autoReleaseToPool")]
        [SerializeField] private bool _autoReleaseInRealTime;
        
        [Space]
        [SerializeField] private bool _enableOnGet = true;
        [SerializeField] private bool _disableOnRelease = true;
        
        [Space]
        [SerializeField] private bool _releaseOnGameStart;
        [FormerlySerializedAs("_releaseOnGameEnd")]
        [SerializeField] private bool _releaseOnGameEndOrReset;

        private bool _activated;

        private bool _autoRelease;
        private float _currentAutoReleaseDelay;
        private bool _currentAutoReleaseInRealTime;
        private float _autoReleaseTime;

        private bool _releaseWhenInvisible;
        private float _releaseWhenInvisibleDelay;
        private float _releaseWhenInvisibleTime;

        #endregion

        #region Properties

        public ObjectPool Pool
        {
            get
            {
                if (_pool == null)
                {
                    _pool = GetPool();
                }

                return _pool;
            }
            set
            {
                _pool = value;
                PoolTag = _pool != null ? _pool.Tag : string.Empty;
            }
        }
        public string PoolTag
        {
            get => _poolTag;
            set
            {
                _poolTag = value;
                if (_pool == null || _pool.Tag != _poolTag)
                {
                    _pool = GetPool();
                }
            }
        }
        [PropertyOrder(-1), ShowInInspector, ReadOnly, ES3Serializable]
        public bool IsActive
        {
            get => Application.isPlaying && Pool != null && Pool.IsInstanceActive(this);
            set
            {
                if (value)
                {
                    Get();
                }
                else
                {
                    ReleaseToPool();
                }
            }
        }

        #endregion

        #region Events

        [Space]
        [FormerlySerializedAs("onGet")]
        public UnityEvent OnGet = new();
        [FormerlySerializedAs("onRelease")]
        public UnityEvent OnRelease = new();

        public Action OnGetAction;
        public Action OnReleaseAction;

        #endregion

        #region MonoBehaviour Methods

        private void Awake()
        {
            GameManager.GameStarted += OnGameStart;
            GameManager.GameOver += OnGameEndOrReset;
            GameManager.GameReset += OnGameEndOrReset;

            this.DoOnNextFrame(Get);
        }

        private void LateUpdate()
        {
            UpdateAutoRelease();
            UpdateReleaseWhenInvisible();
        }

        private void OnDisable()
        {
            if (UpdateManager.Instance != null)
            {
                UpdateManager.Instance.DoOnNextFrame(() => ReleaseToPool());
            }
        }

        private void OnDestroy()
        {
            GameManager.GameStarted -= OnGameStart;
            GameManager.GameOver -= OnGameEndOrReset;
            GameManager.GameReset -= OnGameEndOrReset;

            if (_pool != null)
            {
                _pool.RemoveInstance(this);
            }
        }

        #endregion

        #region Public Methods

        public void Get()
        {
            if (_activated)
            {
                return;
            }

            _activated = true;

            if (Pool != null)
            {
                Pool.AddActiveInstance(this);
            }

            if (_enableOnGet)
            {
                gameObject.SetActive(true);
            }

            OnGet?.Invoke();
            OnGetAction?.Invoke();

            if (_autoReleaseToPool)
            {
                ReleaseToPool(_autoReleaseDelay, _autoReleaseInRealTime);
            }
        }

        [Button]
        public void ReleaseToPool(float delay, bool inRealTime)
        {
            if (Pool != null)
            {
                if (Pool.Contains(this) && !Pool.IsInstanceActive(this))
                {
                    _activated = false;
                    return;
                }
            }
            else if (!_activated)
            {
                return;
            }

            if (delay > 0f)
            {
                _autoRelease = true;
                _currentAutoReleaseDelay = delay;
                _currentAutoReleaseInRealTime = inRealTime;
                _autoReleaseTime = 0f;
                return;
            }

            _activated = false;

            if (Pool != null)
            {
                Pool.AddInactiveInstance(this);
            }

            _autoRelease = false;
            _releaseWhenInvisible = false;

            OnRelease?.Invoke();
            OnReleaseAction?.Invoke();

            if (_disableOnRelease)
            {
                gameObject.SetActive(false);
            }
        }
        
        public void ReleaseToPool(float delay = 0f)
        {
            ReleaseToPool(delay, false);
        }

        public void ReleaseToPoolWithDelayInRealtime(float delay)
        {
            ReleaseToPool(delay, true);
        }

        public void ReleaseToPoolWhenInvisible(float delay = 0f)
        {
            _releaseWhenInvisible = true;
            _releaseWhenInvisibleDelay = delay;
            _releaseWhenInvisibleTime = 0f;
        }

        public static implicit operator GameObject(PooledObject pooledObject) => pooledObject.gameObject;

        #endregion

        #region Private Methods

        private void UpdateAutoRelease()
        {
            if (!_autoRelease)
            {
                return;
            }

            if (_autoReleaseTime >= _currentAutoReleaseDelay)
            {
                ReleaseToPool();
            }
            else if (_currentAutoReleaseInRealTime)
            {
                _autoReleaseTime += Time.unscaledDeltaTime;
            }
            else if (GameManager.InAnyGameState)
            {
                _autoReleaseTime += Time.deltaTime;
            }
        }

        private void UpdateReleaseWhenInvisible()
        {
            if (!_releaseWhenInvisible)
            {
                return;
            }

            if (_releaseWhenInvisibleTime >= _releaseWhenInvisibleDelay && !Pool.IsInstanceVisible(this))
            {
                ReleaseToPool();
            }
            else if (GameManager.InAnyGameState)
            {
                _releaseWhenInvisibleTime += Time.deltaTime;
            }
        }

        private ObjectPool GetPool()
        {
            ObjectPool pool = PoolManager.FindPoolWithInstance(gameObject);
            if (pool == null)
            {
                string tag = !string.IsNullOrEmpty(_poolTag) ? _poolTag : gameObject.name;
                pool = PoolManager.GetPool(tag, null);
            }

            return pool;
        }

        private void OnGameStart()
        {
            if (_releaseOnGameStart)
            {
                ReleaseToPool();
            }
        }

        private void OnGameEndOrReset()
        {
            if (_releaseOnGameEndOrReset)
            {
                ReleaseToPool();
            }
        }

        #endregion
    }
}