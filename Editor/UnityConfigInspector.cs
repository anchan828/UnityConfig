using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using UnityEditorInternal;

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

        private Dictionary<string, ReorderableList> reorderableListDic = new Dictionary<string, ReorderableList>();
        private StringBuilder message = new StringBuilder();

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            DrawLayoutsGUI();

            DrawGameViewSizeGroupGUI();

            if (GUILayout.Button("Load Config", "LargeButton"))
            {
                message = new StringBuilder();
                LoadLayouts();
                LoadGameViewSizes();
            }

            EditorGUILayout.TextArea(message.ToString());

            serializedObject.ApplyModifiedProperties();
        }

        void LoadLayouts()
        {
            foreach (var layout in config.layouts)
            {
                if (layout == null)
                    continue;
                message.Append("[Layout] Loading " + layout.name + " ");
                var sourceLayoutPath = AssetDatabase.GetAssetPath(layout);
                var destLayoutPath = Path.Combine(UnityConfigHelper.layoutFolderPath, Path.GetFileName(AssetDatabase.GetAssetPath(layout)));
                File.Copy(sourceLayoutPath, destLayoutPath, true);
                message.AppendLine("loaded.");

                
            }

            InternalEditorUtility.ReloadWindowLayoutMenu();
        }
        void LoadGameViewSizes()
        {
            foreach (var gameViewSizeGroup in config.gameViewSizes)
            {
                foreach (var gameViewSize in gameViewSizeGroup.gameViewSizes)
                {
                    message.Append("[GameViewSize] Loading " + gameViewSize.name + " ");
                    var result = UnityConfigHelper.LoadGameViewSize(gameViewSizeGroup.type, gameViewSize);

                    message.AppendLine((result ? "" : "already") + " loaded.");

                }
            }
        }

        ReorderableList layoutsList = null;

        void DrawLayoutsGUI()
        {
            EditorGUILayout.LabelField("Layouts", UnityConfigHelper.Style.header, GUILayout.Height(32));  
            
            if (layoutsList == null)
            {
                layoutsList = new ReorderableList(serializedObject, serializedObject.FindProperty("layouts"));
                layoutsList.drawHeaderCallback += (position) => EditorGUI.LabelField(position, "Layout Assets");
                
                layoutsList.drawElementCallback += ( rect, index, isActive, isFocused) =>
                {
                    var prop = layoutsList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.BeginChangeCheck();

                    prop.objectReferenceValue = EditorGUI.ObjectField(rect, prop.objectReferenceValue, typeof(Object));

                    if (EditorGUI.EndChangeCheck())
                    {
                        if (Path.GetExtension(AssetDatabase.GetAssetPath(prop.objectReferenceValue)) != ".wlt")
                            prop.objectReferenceValue = null;
                    }

                    // EditorGUI.PropertyField(rect, layoutsList.serializedProperty.GetArrayElementAtIndex(index));
                };

            }
            layoutsList.DoLayoutList();


        }

        int selectedPlatform = 0;
       
        void DrawGameViewSizeGroupGUI()
        {
            EditorGUILayout.LabelField("GameView Size", UnityConfigHelper.Style.header, GUILayout.Height(32));
           

            var displayNames = config.gameViewSizes.Select(v => v.type.ToString()).ToArray();
            selectedPlatform = GUILayout.SelectionGrid((int)selectedPlatform, displayNames, displayNames.Count(), EditorStyles.toolbarButton);

            EditorGUILayout.Space();

           
            var gameViewSizes = serializedObject.FindProperty("gameViewSizes");

            for (int i = 0; i < gameViewSizes.arraySize; i++)
            {

                var gameViewSizeGroup = gameViewSizes.GetArrayElementAtIndex(i);
                var gameViewSizeList = gameViewSizeGroup.FindPropertyRelative("gameViewSizes");
                var typeProp = gameViewSizeGroup.FindPropertyRelative("type");
                var key = typeProp.enumNames[typeProp.enumValueIndex];

                if (displayNames[selectedPlatform] != key)
                    continue;

                if (!reorderableListDic.ContainsKey(key))
                {
                    var reorderableList = new ReorderableList(serializedObject, gameViewSizeList);
                    reorderableList.drawHeaderCallback += (position) => EditorGUI.LabelField(position, key);
                    reorderableList.onAddCallback += (list) =>
                    {
                        list.serializedProperty.arraySize++;
                        var newProp = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                        newProp.FindPropertyRelative("name").stringValue = "New Game Size";
                        newProp.FindPropertyRelative("sizeType").enumValueIndex = 0;

                    };

                    reorderableList.drawElementCallback += ( rect, index, isActive, isFocused) =>
                    {
                        EditorGUI.PropertyField(rect, reorderableList.serializedProperty.GetArrayElementAtIndex(index));
                    };
                    
                    reorderableListDic.Add(key, reorderableList);

                }
                var _reorderableList = reorderableListDic[key];

                _reorderableList.elementHeight = _reorderableList.count == 0 ? EditorGUIUtility.singleLineHeight : (EditorGUIUtility.singleLineHeight * 4 + 8);
                

                _reorderableList.DoLayoutList();
                EditorGUILayout.Space();
            }


           
        }

        void DrawGameViewSizeGUI(int index, params GameViewSize[] gameViewSizes)
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
                            ArrayUtility.Remove<GameViewSize>(ref config.gameViewSizes[index].gameViewSizes, (GameViewSize)o)
                        , gameViewSize);
                    menu.ShowAsContext();
                }
            }
        }
    }
}