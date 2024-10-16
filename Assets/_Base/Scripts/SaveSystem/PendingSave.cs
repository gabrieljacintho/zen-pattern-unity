using FireRingStudio.Extensions;
using UnityEngine;

namespace FireRingStudio.SaveSystem
{
    public class PendingSave : MonoBehaviour
    {
        private void Start()
        {
            SaveManager.DiscardPendingSave();
            this.DoAfterSeconds(StartSaveAsPending, 1f);
        }

        private void OnDisable()
        {
            SaveManager.DiscardPendingSave();
            StopSaveAsPending();
            StopAllCoroutines();
        }

        public void StartSaveAsPending()
        {
            SaveManager.SaveAsPending = true;
        }

        public void StopSaveAsPending()
        {
            SaveManager.SaveAsPending = false;
        }

        public void ApplyPendingSave()
        {
            SaveManager.ApplyPendingSave();
        }

        public void ApplyAndStopPendingSave()
        {
            SaveManager.ApplyPendingSave();
            StopSaveAsPending();
        }
    }
}