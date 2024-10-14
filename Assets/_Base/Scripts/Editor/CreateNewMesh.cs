using UnityEditor;
using UnityEngine;

namespace FireRingStudio.Editor
{
    public class CreateNewMesh
    {
        [MenuItem("Assets/Create Empty Mesh")]
        public static void Create()
        {
            string filePath = EditorUtility.SaveFilePanelInProject("Save New Mesh", "New Mesh", "mesh", "");
            if (filePath == "")
            {
                return;
            }

            AssetDatabase.CreateAsset(new Mesh(), filePath);
            AssetDatabase.Refresh();
        }
    }
}
