using FireRingStudio.SaveSystem;
using Sirenix.OdinInspector;
using System;

namespace FireRingStudio.LevelManagement
{
    [Serializable]
    public abstract class Level
    {
        public string Id;

        [ShowInInspector, ReadOnly, PropertyOrder(1)] private bool _completed;
        private bool _loaded;

        public bool Completed
        {
            get
            {
                if (!_loaded)
                {
                    LoadSave();
                }

                return _completed;
            }
        }


        public abstract void Load();

        public abstract void Unload();

        public abstract bool IsLoaded();

        public virtual void Complete()
        {
            _completed = true;
            Save();
            Debug.Log("Level completed: \"" + Id + "\"");
        }

        public void ResetLevel()
        {
            _completed = false;
            _loaded = true;
            SaveManager.DeleteKey(Id);
        }

        public void LoadSave()
        {
            _completed = SaveManager.GetBool(Id);
            _loaded = true;
        }

        public void Save()
        {
            SaveManager.SetBool(Id, _completed);
        }
    }
}