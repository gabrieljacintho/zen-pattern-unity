using System.Collections.Generic;
using FireRingStudio.Save;
using I2.Loc;
using UnityEngine;
using UnityEngine.Serialization;

namespace FireRingStudio.UI.Settings
{
    public class FullScreenModeSwitcher : SettingsSwitcher
    {
        [FormerlySerializedAs("availableFullScreenModes")]
        [SerializeField] private List<KeyValue<FullScreenMode, LocalizedString>> _fullScreenModes;

        protected override int ValuesLength => _fullScreenModes != null ? _fullScreenModes.Count : 0;
        protected override int AppliedIndex => ValuesLength > 0 ? _fullScreenModes.FindIndex(x => x.Key == UnityEngine.Screen.fullScreenMode) : 0;
        protected override int DefaultIndex => GetDefaultIndex();


        protected override string GetText(int index)
        {
            return _fullScreenModes[index].Value;
        }
        
        protected override void Apply(int index)
        {
            Screen.fullScreenMode = _fullScreenModes[index].Key;
        }

        private int GetDefaultIndex()
        {
            if (_fullScreenModes == null)
            {
                return 0;
            }

            FullScreenMode defaultFullScreenMode = FullScreenModeSave.FullScreenModes[FullScreenModeSave.DefaultIndex];
            return _fullScreenModes.FindIndex(x => x.Key == defaultFullScreenMode);
        }
    }
}