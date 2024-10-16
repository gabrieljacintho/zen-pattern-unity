using UnityEngine;

namespace FireRingStudio.LevelManagement
{
    public class ObjectLevelManager : LevelManagerGeneric<ObjectLevel>
    {
        [SerializeField] private Transform _parent;


        public override void LoadLevel(Level level)
        {
            if (!string.IsNullOrEmpty(_lastLevelId))
            {
                Level lastLevel = FindLevelWithId(_lastLevelId);
                if (lastLevel != null)
                {
                    lastLevel.Unload();
                }
            }

            ObjectLevel objectLevel = level as ObjectLevel;
            objectLevel.Load(_parent);

            _lastLevelId = level.Id;
            Save();

            OnLevelLoad?.Invoke(level);
        }
    }
}