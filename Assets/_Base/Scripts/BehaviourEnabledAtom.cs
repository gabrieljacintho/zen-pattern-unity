using UnityAtoms.BaseAtoms;
using UnityEngine;

namespace FireRingStudio
{
    public class BehaviourEnabledAtom : MonoBehaviour
    {
        [SerializeField] private MonoBehaviour _behaviour;
        [SerializeField] private BoolVariable _enabledVariable;


        private void OnEnable()
        {
            if (_enabledVariable != null)
            {
                _enabledVariable.Changed.Register(SetEnabled);
            }
            SetEnabled(_enabledVariable != null && _enabledVariable.Value);
        }

        private void OnDisable()
        {
            if (_enabledVariable != null)
            {
                _enabledVariable.Changed.Unregister(SetEnabled);
            }
        }

        private void SetEnabled(bool value)
        {
            if (_behaviour != null)
            {
                _behaviour.enabled = value;
            }
        }
    }
}