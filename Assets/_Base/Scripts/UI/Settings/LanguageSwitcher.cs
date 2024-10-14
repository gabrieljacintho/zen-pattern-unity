using System.Collections.Generic;
using I2.Loc;

namespace FireRingStudio.UI.Settings
{
    public class LanguageSwitcher : SettingsSwitcher
    {
        private List<string> _languages;

        private List<string> Languages
        {
            get
            {
                if (_languages == null)
                {
                    _languages = LocalizationManager.GetAllLanguages();
                }

                return _languages;
            }
        }
        
        protected override int ValuesLength => Languages.Count;
        protected override int AppliedIndex => Languages.IndexOf(LocalizationManager.CurrentLanguage);
        protected override int DefaultIndex => AppliedIndex;

        
        protected override string GetText(int index)
        {
            return LocalizationManager.GetAllLanguages()[index];
        }
        
        protected override void Apply(int index)
        {
            LocalizationManager.CurrentLanguage = LocalizationManager.GetAllLanguages()[index];
        }
    }
}