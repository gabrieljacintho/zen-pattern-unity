using UnityEngine;

namespace FireRingStudio.UI.Options
{
    [RequireComponent(typeof(SelectableOptions))]
    public abstract class SelectableOption : MonoBehaviour
    {
        [SerializeField] private SelectableOptionData _data;

        public SelectableOptionData Data => _data;
        public virtual bool IsExecuting => false;


        public abstract void Execute();
        
        public virtual void ConfirmChanges() { }
        
        public virtual void CancelChanges() { }

        public abstract bool IsAvailable();
    }
}