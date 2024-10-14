using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace FireRingStudio
{
    public class FadeRenderer : MonoBehaviour
    {
        [SerializeField] private RendererSettings[] _rendererSettingsArray = new RendererSettings[1];
        [SerializeField, Range(0f, 1f)] private float _fromAlpha = 1f;
        [SerializeField, Range(0f, 1f)] private float _toAlpha;
        [SerializeField] private float _duration = 1f;
        
        [Space]
        [SerializeField] private bool _startOnEnable;
        [SerializeField] private bool _stopOnDisable;

        private float _t;

        public bool IsFading { get; private set; }
        
        [Space]
        [FormerlySerializedAs("onStart")]
        public UnityEvent OnStart;
        [FormerlySerializedAs("onEnd")]
        public UnityEvent OnEnd;
        
        
        private void OnEnable()
        {
            if (_startOnEnable)
            {
                StartFade();
            }
        }

        private void Update()
        {
            if (!IsFading || _rendererSettingsArray == null)
            {
                return;
            }

            if (_t < 1f)
            {
                _t += Time.deltaTime / _duration;
            }
            else
            {
                EndFade();
            }

            float alpha = Mathf.Lerp(_fromAlpha, _toAlpha, _t);
            
            foreach (RendererSettings rendererSettings in _rendererSettingsArray)
            {
                rendererSettings?.SetAlpha(alpha);
            }
        }
        
        private void OnDisable()
        {
            if (_stopOnDisable)
            {
                StopFade();
            }
        }

        private void OnDestroy()
        {
            DestroyMaterials();
        }

        public void StartFade()
        {
            SetRendererMaterialsToFade();
            
            IsFading = true;
            
            _t = 0f;
            
            OnStart?.Invoke();
        }
        
        public void StopFade()
        {
            IsFading = false;
        }
        
        public void ResetRenderer()
        {
            StopFade();
            SetRendererMaterialsToDefault();
        }
        
        private void EndFade()
        {
            StopFade();
            
            OnEnd?.Invoke();
        }

        private void SetRendererMaterialsToFade()
        {
            if (_rendererSettingsArray == null)
            {
                return;
            }
            
            foreach (RendererSettings rendererSettings in _rendererSettingsArray)
            {
                rendererSettings?.SetMaterialsToFade();
            }
        }
        
        private void SetRendererMaterialsToDefault()
        {
            if (_rendererSettingsArray == null)
            {
                return;
            }
            
            foreach (RendererSettings rendererSettings in _rendererSettingsArray)
            {
                rendererSettings?.SetMaterialsToDefault();
            }
        }
        
        private void DestroyMaterials()
        {
            if (_rendererSettingsArray == null)
            {
                return;
            }
            
            foreach (RendererSettings rendererSettings in _rendererSettingsArray)
            {
                rendererSettings.DestroyMaterials();
            }
        }
        
        [Button]
        private void LoadChildrenRenderers(bool includeInactive)
        {
            Renderer[] renderers = GetComponentsInChildren<Renderer>(includeInactive);

            _rendererSettingsArray = new RendererSettings[renderers.Length];
            for (int i = 0; i < renderers.Length; i++)
            {
                _rendererSettingsArray[i] = new RendererSettings(renderers[i], new MaterialSettings[1]);
            }
        }
    }
}