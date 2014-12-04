using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;

namespace Kyusyukeigo.Helper
{
    [InitializeOnLoad]
    internal class UnityConfigHelper
    {

        static UnityConfigHelper()
        {
        }

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
                return FileUtil.GetProjectRelativePath(currentFolderPath) + "/conf";
            }
        }

        private static string currentFolderPath
        {
            get
            {
                var currentFilePath = new System.Diagnostics.StackTrace(true).GetFrame(0).GetFileName();

                return Path.GetDirectoryName(currentFilePath);
            }
        }

        internal static bool LoadGameViewSize(GameViewSizeGroupType groupType, UnityConfig.GameViewSize gameViewSize)
        {
            if (GameViewSizeHelper.Contains(groupType, gameViewSize.sizeType, gameViewSize.width, gameViewSize.height, gameViewSize.name))
              
                return false;
            else{
                  GameViewSizeHelper.AddCustomSize(groupType, gameViewSize.sizeType, gameViewSize.width, gameViewSize.height, gameViewSize.name);
                return true;
            }
        }
    }
}