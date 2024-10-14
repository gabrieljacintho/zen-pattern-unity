using System;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class ResolutionSave : SettingsSave<int>
    {
        public static int DefaultIndex => Screen.resolutions.Length - 1;
        
        public override string SaveKey => "Resolution";
        public override int DefaultValue => DefaultIndex;
        public override int CurrentValue => Array.IndexOf(Screen.resolutions, Screen.currentResolution);
        

        protected override void Load(int value)
        {
            Resolution resolution = Screen.resolutions[value];
            FullScreenMode fullScreenMode = Screen.fullScreenMode;
            
            Screen.SetResolution(resolution.width, resolution.height, fullScreenMode, resolution.refreshRateRatio);
        }
    }
}