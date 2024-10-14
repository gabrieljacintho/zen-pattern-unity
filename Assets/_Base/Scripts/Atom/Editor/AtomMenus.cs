using System;
using FireRingStudio.Helpers;
using UnityEditor;

namespace FireRingStudio.Atom.Editor
{
    public static class AtomMenus
    {
        [MenuItem("Tools/Unity Atoms/Update All Atom Spreadsheets")]
        public static void UpdateAll()
        {
            AtomSpreadsheet[] atomSpreadsheets = ResourcesHelper.LoadAll<AtomSpreadsheet>();
            foreach (AtomSpreadsheet atomSpreadsheet in atomSpreadsheets)
            {
                try
                {
                    atomSpreadsheet.Update(false);
                }
                catch (Exception error)
                {
                    Debug.LogError("Atom spreadsheet \"" + atomSpreadsheet.name + "\" update failed! " + error.Message);
                }
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            
            Debug.Log("All atom spreadsheets updated successfully. (" + atomSpreadsheets.Length + ")");
        }
    }
}