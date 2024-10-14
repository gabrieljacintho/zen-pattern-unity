using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FireRingStudio.Save
{
    public class FullScreenModeSave : SettingsSave<int>
    {
        private static List<FullScreenMode> s_fullScreenModesList;

        public static int DefaultIndex => FullScreenModes.IndexOf(FullScreenMode.ExclusiveFullScreen);
        
        public override string SaveKey => "FullScreenMode";
        public override int DefaultValue => DefaultIndex;
        public override int CurrentValue => FullScreenModes.IndexOf(Screen.fullScreenMode);

        public static List<FullScreenMode> FullScreenModes
        {
            get
            {
                if (s_fullScreenModesList == null)
                {
                    s_fullScreenModesList = Enum.GetValues(typeof(FullScreenMode)).Cast<FullScreenMode>().ToList();
                }

                return s_fullScreenModesList;
            }
        }


        protected override void Load(int value)
        {
            Screen.fullScreenMode = FullScreenModes[value];
        }
    }
}