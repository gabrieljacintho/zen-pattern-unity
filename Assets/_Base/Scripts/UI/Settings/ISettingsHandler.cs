using UnityEngine;

namespace FireRingStudio.UI.Settings
{
    public interface ISettingsHandler
    {
        GameObject GameObject { get; }

        void Apply();

        void Cancel();

        void ResetSettings();
        
        bool IsApplied();

        bool IsDefault();

        bool HasChanged();
    }
}