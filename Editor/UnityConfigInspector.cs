using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text;

namespace Kyusyukeigo.Helper
{
    [CustomEditor(typeof(UnityConfig))]
    public class UnityConfigInspector : Editor
    {

        UnityConfig config
        {
            get
            {
                return (UnityConfig)target;
            }
        }

        StringBuilder message = new StringBuilder();

        public override void OnInspectorGUI()
        {
            DrawGameViewSizeGroupGUI();

            if (GUILayout.Button("Load Config", "LargeButton"))
            {
                message = new StringBuilder();
                LoadGameViewSizes();
            }

            EditorGUILayout.TextArea(message.ToString());
        }


        void LoadGameViewSizes()
        {
            foreach (var gameViewSizeGroup in config.gameViewSizes)
            {
                foreach (var gameViewSize in gameViewSizeGroup.gameViewSizes)
                {
                    message.Append("Loading [GameViewSize] " + gameViewSize.name + " ");
                    var result = UnityConfigHelper.LoadGameViewSize(gameViewSizeGroup.type, gameViewSize);

                    message.AppendLine((result ? "" : "already") + " loaded.");

                }
            }
        }

        void DrawGameViewSizeGroupGUI()
        {
            for (int i = 0; i < config.gameViewSizes.Length; i++)
            {
                var gameViewSizeGroup = config.gameViewSizes[i];
                EditorGUILayout.BeginVertical("dragtabdropwindow");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(gameViewSizeGroup.type.ToString(), new GUIStyle("HeaderLabel"));
                if (GUILayout.Button("Add", EditorStyles.miniButton))
                {
                    ArrayUtility.Add<UnityConfig.GameViewSize>(ref gameViewSizeGroup.gameViewSizes, new UnityConfig.GameViewSize());
                }
                EditorGUILayout.EndHorizontal();
                DrawGameViewSizeGUI(i, gameViewSizeGroup.gameViewSizes);
                EditorGUILayout.EndVertical();
            }
        }


        void DrawGameViewSizeGUI(int index, params UnityConfig.GameViewSize[] gameViewSizes)
        {

            foreach (var gameViewSize in gameViewSizes)
            {
                var rect = EditorGUILayout.BeginVertical("box");
                gameViewSize.name = EditorGUILayout.TextField("Label", gameViewSize.name);
                gameViewSize.sizeType = (GameViewSizeHelper.GameViewSizeType)EditorGUILayout.EnumPopup("Type", gameViewSize.sizeType);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Width & Height", GUILayout.Width(128));
                gameViewSize.width = EditorGUILayout.IntField(gameViewSize.width);
                gameViewSize.height = EditorGUILayout.IntField(gameViewSize.height);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();


                if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
                {
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Delete"), false, (o) =>
                            ArrayUtility.Remove<UnityConfig.GameViewSize>(ref config.gameViewSizes[index].gameViewSizes, (UnityConfig.GameViewSize)o)
                        , gameViewSize);
                    menu.ShowAsContext();
                }
            }
        }
    }
}