using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace FireRingStudio.FMODIntegration
{
    public class FMODAudioVolumeControl : MonoBehaviour
    {
        [SerializeField] private string _vcaName;

        private string _currentVCAName;
        private VCA _vca;

        public string VCAName
        {
            get => _vcaName;
            set => SetVCAName(value);
        }
        
        public VCA VCA
        {
            get
            {
                if (_currentVCAName != _vcaName)
                {
                    SetVCAName(_vcaName);
                }

                return _vca;
            }
        }
        public float Volume
        {
            get => GetVolume();
            set => SetVolume(value);
        }

        
        public float GetVolume()
        {
            if (!VCA.isValid())
            {
                return 0f;
            }

            VCA.getVolume(out float volume);
            return volume;
        }

        public void SetVolume(float volume)
        {
            if (!VCA.isValid())
            {
                return;
            }
            
            volume = Mathf.Clamp(volume, 0f, 1f);
            VCA.setVolume(volume);
        }

        public void SetVCAName(string value)
        {
            _currentVCAName = value;
            
            _vca = RuntimeManager.GetVCA("vca:/" + _currentVCAName);
            if (!_vca.isValid())
            {
                Debug.LogHasNoWithLabel("FMOD Project", "VCA", "name", _currentVCAName);
            }
        }
    }
}