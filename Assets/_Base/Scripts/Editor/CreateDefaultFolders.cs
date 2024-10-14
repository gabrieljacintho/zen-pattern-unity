using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

namespace FireRingStudio.Editor
{
    public class CreateDefaultFolders : EditorWindow
    {
        private static string s_rootFolder = "DefaultFolders";

        [MenuItem("Assets/Create Default Folders")]
        private static void CreateWindow()
        {
            CreateDefaultFolders window = CreateInstance<CreateDefaultFolders>();
            window.position = new Rect(Screen.width / 2f, Screen.height/ 2f, 400f, 86f);
            window.ShowPopup();
        }
        
        private static void CreateAllFolders()
        {
            string rootFolderPath = "Assets\\" + s_rootFolder;

            List<string> folderPathsToAddKeepFile = new List<string>();
            
            List<string> subfolderNames = new List<string>
            {
                "Art",
                "Audio",
                "Database",
                "Docs",
                "Plugins",
                "Prefabs",
                "Resources",
                "Scenes",
                "Scripts",
                "Settings",
                "ThirdParty"
            };

            List<string> paths = CreateFolders(rootFolderPath, subfolderNames);
            folderPathsToAddKeepFile.AddRange(paths);

            List<string> artSubfolderNames = new List<string>
            {
                "Animations",
                "Fonts",
                "GUI",
                "Materials",
                "Models",
                "Particles",
                "Shaders",
                "Textures"
            };

            string artFolderPath = rootFolderPath + "\\Art";
            paths = CreateFolders(artFolderPath, artSubfolderNames);
            
            folderPathsToAddKeepFile.Remove(artFolderPath);
            folderPathsToAddKeepFile.AddRange(paths);
            
            List<string> audioSubfolderNames = new List<string>
            {
                "Ambient",
                "Music",
                "SFX"
            };
            
            string audioFolderPath = rootFolderPath + "\\Audio";
            paths = CreateFolders(audioFolderPath, audioSubfolderNames);
            
            folderPathsToAddKeepFile.Remove(audioFolderPath);
            folderPathsToAddKeepFile.AddRange(paths);

            foreach (string path in folderPathsToAddKeepFile)
            {
                CreateKeepFile(path);
            }

            AssetDatabase.Refresh();
        }

        private static string CreateFolder(string rootFolderPath, string folderName)
        {
            string path = rootFolderPath + "\\" + folderName;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private static List<string> CreateFolders(string path, List<string> folderNames)
        {
            List<string> paths = new List<string>();
            foreach (string folderName in folderNames)
            {
                string folderPath = CreateFolder(path, folderName);
                paths.Add(folderPath);
            }

            return paths;
        }
        
        private static void CreateKeepFile(string path)
        {
            path += "\\.keep";
            if (!File.Exists(path))
            {
                File.Create(path);
            }
        }

        private void OnGUI()
        {
            s_rootFolder = EditorGUILayout.TextField("Root folder name", s_rootFolder);
            Repaint();
            
            GUILayout.Space(20);
            
            if (GUILayout.Button("Create"))
            {
                CreateAllFolders();
                Close();
            }
            
            if (GUILayout.Button("Cancel"))
            {
                Close();
            }
        }
    }
}