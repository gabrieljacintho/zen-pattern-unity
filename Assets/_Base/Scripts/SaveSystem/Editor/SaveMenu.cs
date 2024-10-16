using UnityEditor;

namespace FireRingStudio.SaveSystem.Editor
{
    public static class SaveMenu
    {
        [MenuItem("Tools/Save/Open Save Directory")]
        public static void OpenSaveDirectory()
        {
            string path = SaveManager.FilePath.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", "/select," + path);
        }
    }
}