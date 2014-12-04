using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

namespace Kyusyukeigo.Helper
{
	
    public class UnityConfig : ScriptableObject
    {
        public UnityConfig.GameSizeGroup[] gameViewSizes = new UnityConfig.GameSizeGroup[0];

        [System.Serializable]
        public class GameSizeGroup
        {
            [HideInInspector]
            public GameViewSizeGroupType type;
            public UnityConfig.GameViewSize[] gameViewSizes = new UnityConfig.GameViewSize[0];
        }

        [System.Serializable]
        public class GameViewSize
        {
            public string name = "New Game Size";
            public GameViewSizeHelper.GameViewSizeType sizeType;
            public int width;
            public int height;
        }


        [MenuItem("Assets/Create/UnityConfig/New Config")]
        static void CreateConfig()
        {
            var config = CreateInstance<UnityConfig>();

            foreach (var gameViewSizeType in UnityConfigHelper.gameViewSizeTypes)
            {
                ArrayUtility.Add<GameSizeGroup>(ref config.gameViewSizes, new GameSizeGroup
                    {
                        type = gameViewSizeType
                    });
            }
            ProjectWindowUtil.CreateAsset(config, UnityConfigHelper.configFolderPath + "/New UnityConfig.asset");
        }
    }
}