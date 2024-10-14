using System.Collections.Generic;

namespace FireRingStudio.Save
{
    public static class SaveManager
    {
        private static readonly List<ISave> _saves = new List<ISave>();


        public static void RegisterSave(ISave save)
        {
            if (!_saves.Contains(save))
            {
                _saves.Add(save);
            }
        }

        public static void UnregisterSave(ISave save)
        {
            _saves.Remove(save);
        }

        public static void Save(ES3Settings settings)
        {
            foreach (ISave save in _saves)
            {
                save.Save(settings);
            }

            Debug.Log("Game saved.");
        }

        public static void Save() => Save(new ES3Settings(ES3.Location.File));

        public static void Load(ES3Settings settings)
        {
            foreach (ISave save in _saves)
            {
                save.Load(settings);
            }

            Debug.Log("Game loaded.");
        }

        public static void Load() => Load(new ES3Settings(ES3.Location.File));
    }
}