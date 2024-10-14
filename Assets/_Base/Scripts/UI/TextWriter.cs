using System;
using FMODUnity;
using FireRingStudio.Extensions;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Sirenix.OdinInspector;

namespace FireRingStudio.UI
{
    [ExecuteAlways]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextWriter : MonoBehaviour
    {
        [SerializeField] private bool _usedLocalizedText;
        [HideIf("_usedLocalizedText")]
        [SerializeField] private string _targetText;
        [ShowIf("_usedLocalizedText")]
        [SerializeField] private LocalizedString _localizedTargetText;
        [SerializeField, Range(0f, 1f)] private float _fillAmount;
        [SerializeField, Min(0f)] private float _charsPerMinute = 280f;
        [SerializeField] private bool _restoreOnDisable;

        [Header("Audio")]
        [SerializeField] private EventReference _writingAudio;

        [Space]
        public UnityEvent OnEnd;

        private TextMeshProUGUI _text;
        private StudioEventEmitter _writingAudioSource;

        private float _lastFillAmount;
        
        public string TargetText
        {
            get => _usedLocalizedText ? _localizedTargetText : _targetText;
            set
            {
                if (_usedLocalizedText)
                {
                    _localizedTargetText = value;
                }
                else
                {
                    _targetText = value;
                }
                UpdateText();
            }
        }
        public float FillAmount
        {
            get => _fillAmount;
            set => SetFillAmount(value);
        }
        
        private float FillAmountPerChar => 1f / TargetText.ToString().Length;
        
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!Application.isPlaying)
            {
                UpdateText();
                return;
            }

            if (_charsPerMinute != 0f)
            {
                float charCount = Time.deltaTime / 60f * _charsPerMinute;
                FillAmount += charCount * FillAmountPerChar;
            }
        }

        private void LateUpdate()
        {
            if (!Application.isPlaying)
            {
                return;
            }
            
            if (Math.Abs(FillAmount - _lastFillAmount) > 0.000001f)
            {
                PlayWritingAudio();
            }
            else
            {
                StopWritingAudio();
            }

            if (FillAmount >= 0.99f && _lastFillAmount < 0.99f)
            {
                OnEnd?.Invoke();
            }

            _lastFillAmount = FillAmount;
            
            UpdateText();
        }

        private void OnDisable()
        {
            if (_restoreOnDisable)
            {
                Restore();
            }

            ReleaseWritingAudioToPool();
        }

        public void SetFillAmount(float value)
        {
            _fillAmount = Mathf.Clamp(value, 0f, 1f);
            UpdateText();
        }
        
        public void Restore()
        {
            _fillAmount = 0f;
            _text.text = null;
        }

        private void UpdateText()
        {
            string text = TargetText;
            _text.text = text.Substring(0, Mathf.CeilToInt(text.Length * _fillAmount));
        }
        
        private void PlayWritingAudio()
        {
            if (_writingAudio.IsNull)
            {
                return;
            }
            
            if (_writingAudioSource == null)
            {
                _writingAudioSource = _writingAudio.Play(false, false);
            }
            else if (!_writingAudioSource.IsPlaying())
            {
                _writingAudioSource.Play();
            }
        }

        private void StopWritingAudio()
        {
            if (_writingAudioSource == null || !_writingAudioSource.IsPlaying())
            {
                return;
            }
            
            _writingAudioSource.Stop();
        }

        private void ReleaseWritingAudioToPool()
        {
            if (_writingAudioSource == null)
            {
                return;
            }

            _writingAudioSource.ReleaseToPool();
            _writingAudioSource = null;
        }
    }
}