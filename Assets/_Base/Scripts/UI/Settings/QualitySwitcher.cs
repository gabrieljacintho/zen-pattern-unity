using FireRingStudio.Save;
using I2.Loc;
using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    public class QualitySwitcher : SettingsSwitcher
    {
        [SerializeField] private bool _applyExpensiveChanges;
        [SerializeField] private string _localizationPath = "Graphics";
        
        protected override int ValuesLength => QualitySettings.names.Length;
        protected override int AppliedIndex => QualitySettings.GetQualityLevel();
        protected override int DefaultIndex => QualitySave.DefaultLevel;


        protected override string GetText(int index)
        {
            string text = QualitySettings.names[index];

            string term = text.Replace(" ", "");
            if (string.IsNullOrEmpty(_localizationPath))
            {
                if (LocalizationManager.TryGetTranslation(term, out string translation))
                {
                    text = translation;
                }
            }
            else
            {
                if (LocalizationManager.TryGetTranslation(_localizationPath + "/" + term, out string translation))
                {
                    text = translation;
                }
            }
            
            return text;
        }
        
        protected override void Apply(int index)
        {
            QualitySettings.SetQualityLevel(index, _applyExpensiveChanges);
        }
    }
}