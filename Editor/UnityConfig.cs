using System.IO;
using UnityEngine;
using UnityEditor;

namespace Kyusyukeigo.Helper
{
    [System.Serializable]
    public class GameSizeGroup
    {
        [HideInInspector]
        public GameViewSizeGroupType type;

        public GameViewSize[] gameViewSizes = new GameViewSize[0];
    }

    [System.Serializable]
    public class GameViewSize
    {
        public string name = "New Game Size";
        public GameViewSizeHelper.GameViewSizeType sizeType;
        public int width;
        public int height;
    }

    [System.Serializable]
    public class Preferences
    {
        #region General

        public bool kAutoRefresh = true;
        public bool alwaysShowProjectWizard;
        public bool kCompressTexturesOnImport = true;
        public bool useOSColorPicker;
        public bool enableEditorAnalytics = true;
        public bool showAssetStoreSearchHits = true;
        public bool verifySavingAssets;
        public bool allowAlphaNumericHierarchy;

        #endregion General

        #region External Tools

        public string kScriptsDefaultApp;
        public bool allowAttachedDebuggingOfEditor = true;
        public string kScriptEditorArgs;
        public string kImagesDefaultApp;
        public string kDiffsDefaultApp;
        public string androidSdkRoot;

        #endregion External Tools

        #region Cache Server

        public bool useCacheServer;
        public string IPAddress;

        #endregion

    }

    public class UnityConfig : ScriptableObject
    {
        public GameSizeGroup[] gameViewSizes = new GameSizeGroup[0];

        public Object[] layouts = new Object[0];

        public Preferences preferences = new Preferences();

        [MenuItem("Assets/Create/UnityConfig/New Config")]
        static void CreateConfig()
        {
            var config = CreateInstance<UnityConfig>();

            config.Reset();
            Directory.CreateDirectory(UnityConfigHelper.configFolderPath);
            Debug.Log(UnityConfigHelper.configFolderPath);
            ProjectWindowUtil.CreateAsset(config, Path.Combine(UnityConfigHelper.configFolderPath, "New UnityConfig.asset"));
        }

        void Reset()
        {
            gameViewSizes = new GameSizeGroup[0];

            foreach (var gameViewSizeType in new[] { GameViewSizeGroupType.WebPlayer, GameViewSizeGroupType.Standalone, GameViewSizeGroupType.Android, GameViewSizeGroupType.iOS })
            {
                ArrayUtility.Add(ref gameViewSizes, new GameSizeGroup
                    {
                        type = gameViewSizeType
                    });
            }
        }
    }
}