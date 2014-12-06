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

        public bool kAutoRefresh;
        public bool alwaysShowProjectWizard;
        public bool kCompressTexturesOnImport;
        public bool useOSColorPicker;
        public bool enableEditorAnalytics;
        public bool showAssetStoreSearchHits;
        public bool verifySavingAssets;
        public bool allowAlphaNumericHierarchy;

        #endregion General

        #region External Tools

        public string kScriptsDefaultApp;
        public bool allowAttachedDebuggingOfEditor;
        public string kScriptEditorArgs;
        public string kImagesDefaultApp;
        public string kDiffsDefaultApp;
        public string androidSdkRoot;

        #endregion External Tools

        #region Cache Server

        public bool useCacheServer;
        public string cacheServerIPAddress;

        #endregion

    }

    public class UnityConfig : ScriptableObject
    {
        public GameSizeGroup[] gameViewSizes = {
         new GameSizeGroup  { type = GameViewSizeGroupType.WebPlayer },
         new GameSizeGroup  { type = GameViewSizeGroupType.Standalone },
         new GameSizeGroup  { type = GameViewSizeGroupType.Android },
         new GameSizeGroup  { type = GameViewSizeGroupType.iOS },
         new GameSizeGroup  { type = GameViewSizeGroupType.WebPlayer },
        };

        public Object[] layouts = new Object[0];

        public Preferences preferences = new Preferences
        {
            // General

            kAutoRefresh = true,
            alwaysShowProjectWizard = false,
            kCompressTexturesOnImport = true,
            useOSColorPicker = false,
            enableEditorAnalytics = true,
            showAssetStoreSearchHits = true,
            verifySavingAssets = false,
            allowAlphaNumericHierarchy = false,

            // External Tools

            kScriptsDefaultApp = "",
            allowAttachedDebuggingOfEditor = true,
            kScriptEditorArgs = "",
            kImagesDefaultApp = "",
            kDiffsDefaultApp = "",
            androidSdkRoot = "",

            // Cache Server

            useCacheServer = false,
            cacheServerIPAddress = ""
        };

        [MenuItem("Assets/Create/UnityConfig/New Config")]
        static void CreateConfig()
        {
            var config = CreateInstance<UnityConfig>();
            ProjectWindowUtil.CreateAsset(config, "New UnityConfig.asset");
        }
    }
}