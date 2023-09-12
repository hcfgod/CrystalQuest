using UnityEditor;

namespace SaveSystem.Editor
{

    public static class SaveGameConstants
    {

        private static string _SaveGamesProjectFolder;

        public static string BayatGamesFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_SaveGamesProjectFolder))
                {
                    _SaveGamesProjectFolder = AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("Game")[0]);
                }
                return _SaveGamesProjectFolder;
            }
        }
        public static string SaveGameProFolder = BayatGamesFolder + "/Scripts/SaveGamePro";
        public static string IntegrationFolder = SaveGameProFolder + "/Integrations";

    }

}