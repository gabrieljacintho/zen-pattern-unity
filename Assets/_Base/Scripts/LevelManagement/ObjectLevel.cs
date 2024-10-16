using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FireRingStudio.LevelManagement
{
    [Serializable]
    public class ObjectLevel : Level
    {
        public LevelObject Prefab;

        private LevelObject _instance;

        public LevelObject Instance => _instance;


        public override void Load()
        {
            Load();
        }

        public LevelObject Load(Transform parent)
        {
            Unload();
            _instance = Object.Instantiate(Prefab, parent);
            _instance.OnLoad();
            return _instance;
        }

        public override void Unload()
        {
            if (!IsLoaded())
            {
                return;
            }

            _instance.OnUnload();
            _instance = null;
        }

        public override bool IsLoaded()
        {
            return _instance != null;
        }

        public override void Complete()
        {
            base.Complete();
            if (IsLoaded())
            {
                _instance.OnComplete();
            }
        }
    }
}
