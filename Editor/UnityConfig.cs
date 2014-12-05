using UnityEngine;
using System.Collections.Generic;
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
  

    public class UnityConfig : ScriptableObject
    {
        public GameSizeGroup[] gameViewSizes = new GameSizeGroup[0];
       
        public Object[] layouts = new Object[0];

        [MenuItem("Assets/Create/UnityConfig/New Config")]
        static void CreateConfig()
        {
            var config = CreateInstance<UnityConfig>();

            config.Reset();

            ProjectWindowUtil.CreateAsset(config, UnityConfigHelper.configFolderPath + "/New UnityConfig.asset");
        }

        void Reset()
        {
            gameViewSizes = new GameSizeGroup[0];

            foreach (var gameViewSizeType in new []{GameViewSizeGroupType.WebPlayer,GameViewSizeGroupType.Standalone,GameViewSizeGroupType.Android,GameViewSizeGroupType.iOS})
            {
                ArrayUtility.Add<GameSizeGroup>(ref gameViewSizes, new GameSizeGroup
                    {
                        type = gameViewSizeType
                    });
            }
        }
    }
}