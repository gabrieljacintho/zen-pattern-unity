using System;
using System.Collections.Generic;
using I2.Loc;
using UnityAtoms;
using UnityEngine;

namespace FireRingStudio.Localization
{
    public class RegisterGlobalParameters : MonoBehaviour, ILocalizationParamsManager
    {
        [Serializable]
        public struct GlobalParameter
        {
            public string Name;
            public AtomBaseVariable Variable;
        }
        
        [SerializeField] private List<GlobalParameter> _parameters;


        private void OnEnable()
        {
            if (!LocalizationManager.ParamManagers.Contains(this))
            {
                LocalizationManager.ParamManagers.Add(this);
                LocalizationManager.LocalizeAll(true);
            }
        }

        private void OnDisable()
        {
            LocalizationManager.ParamManagers.Remove(this);
        }

        public string GetParameterValue(string paramName)
        {
            if (_parameters == null)
            {
                return null;
            }

            GlobalParameter variable = _parameters.Find(param => param.Name == paramName);
            
            return variable.Variable != null ? variable.Variable.BaseValue.ToString() : null;
        }
    }
}