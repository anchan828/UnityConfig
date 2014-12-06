using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
namespace Kyusyukeigo.Helper
{
    [InitializeOnLoad]
    internal class UnityConfigHelper
    {
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
            
            GameViewSizeHelper.AddCustomSize(groupType, gameViewSize.sizeType, gameViewSize.width, gameViewSize.height, gameViewSize.name);
            
            return true;
        }

        internal class Style
        {
            public static GUIStyle header = new GUIStyle(EditorStyles.largeLabel);
            public static GUIStyle lgHeader = new GUIStyle("RL Header");
            public static GUIStyle lgBackground = new GUIStyle("RL Background");
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