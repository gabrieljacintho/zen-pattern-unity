using System.Collections.Generic;
using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio.SaveSystem
{
    public class AtomsSave : MonoBehaviour
    {
        [SerializeField] private List<IntVariable> _intVariables;
        [SerializeField] private List<FloatVariable> _floatVariables;


        private void Awake()
        {
            Load();
            RegisterEvents();
        }

        private void OnDestroy()
        {
            UnregisterEvents();
        }

        private void Load()
        {
            LoadIntVariables();
            LoadFloatVariables();
        }

        #region Int

        private void SaveIntVariables()
        {
            if (_intVariables == null)
            {
                return;
            }

            foreach (IntVariable variable in _intVariables)
            {
                SaveManager.SetInt(variable.Id, variable.Value);
            }
        }

        private void LoadIntVariables()
        {
            if (_intVariables == null)
            {
                return;
            }

            foreach (IntVariable variable in _intVariables)
            {
                variable.Value = SaveManager.GetInt(variable.Id, variable.InitialValue);
            }
        }

        #endregion

        #region Float

        private void SaveFloatVariables()
        {
            if (_floatVariables == null)
            {
                return;
            }

            foreach (FloatVariable variable in _floatVariables)
            {
                SaveManager.SetFloat(variable.Id, variable.Value);
            }
        }

        private void LoadFloatVariables()
        {
            if (_floatVariables == null)
            {
                return;
            }

            foreach (FloatVariable variable in _floatVariables)
            {
                variable.Value = SaveManager.GetFloat(variable.Id, variable.InitialValue);
            }
        }

        #endregion

        private void RegisterEvents()
        {
            if (_intVariables != null)
            {
                foreach (IntVariable variable in _intVariables)
                {
                    variable.Changed.Register(SaveIntVariables);
                }
            }

            if (_floatVariables != null)
            {
                foreach (FloatVariable variable in _floatVariables)
                {
                    variable.Changed.Register(SaveFloatVariables);
                }
            }
        }

        private void UnregisterEvents()
        {
            if (_intVariables != null)
            {
                foreach (IntVariable variable in _intVariables)
                {
                    variable.Changed.Unregister(SaveIntVariables);
                }
            }

            if (_floatVariables != null)
            {
                foreach (FloatVariable variable in _floatVariables)
                {
                    variable.Changed.Unregister(SaveFloatVariables);
                }
            }
        }
    }
}
