using FireRingStudio.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FireRingStudio.FPS.Equipment
{
    [RequireComponent(typeof(Animator))]
    [DisallowMultipleComponent]
    public class EquipmentAnimator : EquipmentComponent
    {
        [SerializeField] private Animator _armsAnimator;
        [HideIf("@_armsAnimator != null")]
        [SerializeField] private string _armsAnimatorId = "arms";
        [SerializeField] private AnimatorOverrideController _armsAnimatorOverride;
        
        private Animator _animator;

        public Animator Animator
        {
            get
            {
                if (_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }

                return _animator;
            }
        }
        public Animator ArmsAnimator
        {
            get
            {
                if (_armsAnimator == null)
                {
                    _armsAnimator = ComponentID.FindComponentWithID<Animator>(_armsAnimatorId, true);
                }

                return _armsAnimator;
            }
        }

        
        private void OnEnable()
        {
            if (ArmsAnimator != null)
            {
                ArmsAnimator.gameObject.SetActive(true);
            }
        }
        
        private void OnDisable()
        {
            if (ArmsAnimator != null)
            {
                ArmsAnimator.gameObject.SetActive(false);
            }
        }
        
        public void Initialize()
        {
            if (ArmsAnimator != null)
            {
                ArmsAnimator.runtimeAnimatorController = _armsAnimatorOverride;
            }
            
            ResetAllTriggers();
        }

        public void SetFloat(int id, float value)
        {
            if (Animator != null)
            {
                Animator.SetFloat(id, value);
            }

            if (ArmsAnimator != null)
            {
                ArmsAnimator.SetFloat(id, value);
            }
        }

        public void SetInteger(int id, int value)
        {
            if (Animator != null)
            {
                Animator.SetInteger(id, value);
            }
            
            if (ArmsAnimator != null)
            {
                ArmsAnimator.SetInteger(id, value);
            }
        }

        public void SetBool(int id, bool value)
        {
            if (Animator != null)
            {
                Animator.SetBool(id, value);
            }

            if (ArmsAnimator != null)
            {
                ArmsAnimator.SetBool(id, value);
            }
        }

        public void SetTrigger(int id)
        {
            if (Animator != null)
            {
                Animator.SetTrigger(id);
            }

            if (ArmsAnimator != null)
            {
                ArmsAnimator.SetTrigger(id);
            }
        }

        public void ResetTrigger(int id)
        {
            if (Animator != null)
            {
                Animator.ResetTrigger(id);
            }

            if (ArmsAnimator != null)
            {
                ArmsAnimator.ResetTrigger(id);
            }
        }
        
        public void ResetAllTriggers()
        {
            if (Animator != null)
            {
                Animator.ResetAllTriggers();
            }

            if (ArmsAnimator != null)
            {
                ArmsAnimator.ResetAllTriggers();
            }
        }
    }
}
