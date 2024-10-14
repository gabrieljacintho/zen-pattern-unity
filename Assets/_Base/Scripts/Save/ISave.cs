namespace FireRingStudio.Save
{
    public interface ISave
    {
        public string SaveKey { get; }

        public void Save(ES3Settings settings);

        public void Load(ES3Settings settings);

        public void DeleteSave(ES3Settings settings);
    }
}