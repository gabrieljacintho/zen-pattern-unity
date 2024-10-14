using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace FireRingStudio.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DynamicText : MonoBehaviour
    {
        [SerializeField, TextArea] public string _targetText;
        [SerializeField] private List<TagValue> _tagValues;
        [SerializeField] private bool _updateOnEnable = true;
        
        private TextMeshProUGUI _text;
        private TextWriter _textWriter;


        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _textWriter = GetComponent<TextWriter>();
        }

        private void OnEnable()
        {
            if (_updateOnEnable)
            {
                UpdateText();
            }
        }

        [Button]
        public void UpdateText()
        {
            string newText = _targetText;

            if (_tagValues != null)
            {
                foreach (TagValue value in _tagValues)
                {
                    string valueTag = value.Tag;
                    string valueText = value.GetText();
                    
                    newText = newText.Replace(valueTag, valueText);
                }
            }

            if (_textWriter != null)
            {
                _textWriter.TargetText = newText;
            }
            else if (_text != null)
            {
                _text.text = newText;
            }
        }
    }
}
