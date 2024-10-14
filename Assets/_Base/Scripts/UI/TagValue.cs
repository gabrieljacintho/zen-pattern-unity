using System;
using I2.Loc;
using Sirenix.OdinInspector;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.UI
{
    [Serializable]
    public enum TagType
    {
        String,
        LocalizedString,
        Float,
        Int,
    }
    
    [Serializable]
    public struct TagValue
    {
        public string Tag;
        [EnumToggleButtons, HideLabel]
        public TagType Type;
        [ShowIf("@Type == TagType.String")]
        public StringVariable StringVariable;
        [ShowIf("@Type == TagType.LocalizedString")]
        public LocalizedString LocalizedString;
        [ShowIf("@Type == TagType.Float")]
        public FloatVariable FloatVariable;
        [ShowIf("@Type == TagType.Float")]
        [Range(0, 3)] public int DecimalDigits;
        [ShowIf("@Type == TagType.Int")]
        public IntVariable IntVariable;
        
        
        public string GetText()
        {
            switch (Type)
            {
                case TagType.String:
                    return StringVariable.Value;
                
                case TagType.LocalizedString:
                    return LocalizedString;
                
                case TagType.Float:
                    string format = "F" + DecimalDigits;
                    return FloatVariable.Value.ToString(format);
                        
                case TagType.Int:
                    return IntVariable.Value.ToString();
                
                default:
                    return null;
            }
        }
    }
}