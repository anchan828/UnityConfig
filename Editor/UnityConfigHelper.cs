using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using UnityEditorInternal;
namespace Kyusyukeigo.Helper
{
    [InitializeOnLoad]
    internal class UnityConfigHelper
    {
        internal static GameViewSizeGroupType[] gameViewSizeTypes
        {
            get
            {
                return Enum.GetValues(typeof(GameViewSizeGroupType)).Cast<GameViewSizeGroupType>().ToArray();
            }
        }

        internal static string[] GetConfigNames()
        {
            return Directory.GetFiles(configFolderPath, "*.json").Select(path => Path.GetFileNameWithoutExtension(path)).ToArray();
        }

        internal static string configFolderPath
        {
            get
            {
                return Path.Combine(currentFolderPath, "conf");
            }
        }

        private static string currentFolderPath
        {
            get
            {
                var csPath = AssetDatabase.GetAllAssetPaths()
                        .FirstOrDefault(path => Path.GetFileName(path) == typeof(UnityConfigHelper).Name + ".cs");
                return Path.GetDirectoryName(csPath);
            }
        }

        internal static string layoutFolderPath
        {
            get
            {
                return InternalEditorUtility.unityPreferencesFolder + "/Layouts";
            }
        }

        internal static bool LoadGameViewSize(GameViewSizeGroupType groupType, GameViewSize gameViewSize)
        {
            if (GameViewSizeHelper.Contains(groupType, gameViewSize.sizeType, gameViewSize.width, gameViewSize.height, gameViewSize.name))
                return false;
            else
            {
                GameViewSizeHelper.AddCustomSize(groupType, gameViewSize.sizeType, gameViewSize.width, gameViewSize.height, gameViewSize.name);
                return true;
            }
        }

        internal class Style
        {
            public static GUIStyle header = new GUIStyle(EditorStyles.largeLabel);

            static Style()
            {
                header.fontStyle = FontStyle.Bold;
                header.fontSize = 18;
                header.margin.top = 10;
                header.margin.left++;
                header.normal.textColor = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f, 1f) : new Color(0.4f, 0.4f, 0.4f, 1f);
            }
        }
    }
}