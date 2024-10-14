using System;
using FireRingStudio.Save;
using I2.Loc;
using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    public class ResolutionSwitcher : SettingsSwitcher
    {
        [SerializeField] private LocalizedString _localizedHz;
        
        protected override int ValuesLength => Screen.resolutions.Length;
        protected override int AppliedIndex => Screen.resolutions != null ? Array.IndexOf(Screen.resolutions, Screen.currentResolution) : 0;
        protected override int DefaultIndex => ResolutionSave.DefaultIndex;


        protected override string GetText(int index)
        {
            string text = Screen.resolutions[index].ToString();
            text = text.Replace("Hz", _localizedHz);

            return text;
        }
        
        protected override void Apply(int index)
        {
            Resolution resolution = Screen.resolutions[index];
            FullScreenMode fullScreenMode = Screen.fullScreenMode;
            
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
        }
    }
}