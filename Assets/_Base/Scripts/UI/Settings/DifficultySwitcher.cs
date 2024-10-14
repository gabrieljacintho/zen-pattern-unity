using FireRingStudio.Difficulty;
using FireRingStudio.Save;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    public class DifficultySwitcher : SettingsSwitcher
    {
        [SerializeField] private bool _overrideLastLevelTextColor;
        [ShowIf("_overrideLastLevelTextColor")]
        [SerializeField] private Color _lastLevelTextColor = Color.red;

        private SelectableGraphic _selectableGraphic;
        
        private Color _defaultTextColor;

        private SelectableGraphic SelectableGraphic
        {
            get
            {
                if (_selectableGraphic == null)
                {
                    _selectableGraphic = GetComponent<SelectableGraphic>();
                }

                return _selectableGraphic;
            }
        }

        protected override int ValuesLength => DifficultyManager.Levels != null ? DifficultyManager.Levels.Length : 0;
        protected override int AppliedIndex => DifficultyManager.CurrentLevel;
        protected override int DefaultIndex => DifficultySave.DefaultLevel;


        protected override void Awake()
        {
            base.Awake();

            if (Text != null)
            {
                _defaultTextColor = Text.color;
            }
        }

        protected override void UpdateText()
        {
            base.UpdateText();
            
            if (Text == null || !_overrideLastLevelTextColor)
            {
                return;
            }

            Color color;
            if (CurrentIndex == ValuesLength - 1)
            {
                color = _lastLevelTextColor;
                
                if (SelectableGraphic != null)
                {
                    SelectableGraphic.NormalColor = color;
                }
            }
            else if (SelectableGraphic != null)
            {
                SelectableGraphic.NormalColor = _defaultTextColor;
                color = SelectableGraphic.GetCurrentStateColor();
            }
            else
            {
                color = _defaultTextColor;
            }

            Text.color = color;
        }

        protected override string GetText(int index)
        {
            return DifficultyManager.Levels[index];
        }
        
        protected override void Apply(int index)
        {
            DifficultyManager.CurrentLevel = index;
        }
    }
}