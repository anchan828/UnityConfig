using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;
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

        private readonly Dictionary<string, ReorderableList> reorderableListDic = new Dictionary<string, ReorderableList>();
        ReorderableList layoutsList;
        int selectedPlatform;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawPreferencesGUI();

            DrawSnapGUI();

            DrawLayoutsGUI();

            DrawGameViewSizeGroupGUI();

            if (GUILayout.Button("Load Config", "LargeButton"))
            {
                LoadSnap();
                LoadPreferences();
                LoadLayouts();
                LoadGameViewSizes();
                InternalEditorUtility.RequestScriptReload();
            }

            serializedObject.ApplyModifiedProperties();
        }

        void LoadSnap()
        {
            var shouldOpen = false;
            var snapSettingsType = Types.GetType("UnityEditor.SnapSettings", "UnityEditor.dll");
            var windows = Resources.FindObjectsOfTypeAll(snapSettingsType);

            foreach (var window in windows.Cast<EditorWindow>())
            {
                shouldOpen = true;
                window.Close();
            }


            var s = config.snap;

            EditorPrefs.SetFloat("MoveSnapX", s.moveSnapX);
            EditorPrefs.SetFloat("MoveSnapY", s.moveSnapY);
            EditorPrefs.SetFloat("MoveSnapZ", s.moveSnapZ);
            EditorPrefs.SetFloat("ScaleSnap", s.scaleSnap);
            EditorPrefs.SetFloat("RotationSnap", s.rotationSnap);

            if (shouldOpen)
                EditorWindow.GetWindowWithRect(snapSettingsType, new Rect(100, 100, 230, 130), true, "Snap settings");
        }

        void LoadPreferences()
        {
            var shouldOpen = false;
            var preferencesWindowType = Types.GetType("UnityEditor.PreferencesWindow", "UnityEditor.dll");
            var windows = Resources.FindObjectsOfTypeAll(preferencesWindowType);

            foreach (var window in windows.Cast<EditorWindow>())
            {
                shouldOpen = true;
                window.Close();
            }

            var p = config.preferences;

            if (!string.IsNullOrEmpty(p.kScriptsDefaultApp))
                EditorPrefs.SetString("kScriptsDefaultApp", p.kScriptsDefaultApp);
            if (!string.IsNullOrEmpty(p.kScriptEditorArgs))
                EditorPrefs.SetString("kScriptEditorArgs", p.kScriptEditorArgs);
            if (!string.IsNullOrEmpty(p.kImagesDefaultApp))
                EditorPrefs.SetString("kImagesDefaultApp", p.kImagesDefaultApp);
            if (!string.IsNullOrEmpty(p.kDiffsDefaultApp))
                EditorPrefs.SetString("kDiffsDefaultApp", p.kDiffsDefaultApp);
            if (!string.IsNullOrEmpty(p.androidSdkRoot))
                EditorPrefs.SetString("AndroidSdkRoot", p.androidSdkRoot);

            EditorPrefs.SetBool("kAutoRefresh", p.kAutoRefresh);
#if ENABLE_HOME_SCREEN
        // EditorPrefs.SetBool("ReopenLastUsedProjectOnStartup", m_ReopenLastUsedProjectOnStartup);
#else
            EditorPrefs.SetBool("AlwaysShowProjectWizard", p.alwaysShowProjectWizard);
#endif

            EditorPrefs.SetBool("kCompressTexturesOnImport", p.kCompressTexturesOnImport);
            EditorPrefs.SetBool("UseOSColorPicker", p.useOSColorPicker);
            EditorPrefs.SetBool("EnableEditorAnalytics", p.enableEditorAnalytics);
            EditorPrefs.SetBool("ShowAssetStoreSearchHits", p.showAssetStoreSearchHits);
            EditorPrefs.SetBool("VerifySavingAssets", p.verifySavingAssets);
            EditorPrefs.SetBool("AllowAttachedDebuggingOfEditor", p.allowAttachedDebuggingOfEditor);

            EditorPrefs.SetBool("AllowAlphaNumericHierarchy", p.allowAlphaNumericHierarchy);


            EditorPrefs.SetBool("CacheServerEnabled", p.useCacheServer);
            EditorPrefs.SetString("CacheServerIPAddress", p.cacheServerIPAddress);

            if (shouldOpen)
                EditorWindow.GetWindowWithRect(preferencesWindowType, new Rect(100, 100, 500, 400), true, "Unity Preferences");
        }

        void LoadLayouts()
        {
            foreach (var layout in config.layouts)
            {
                if (layout == null)
                    continue;
                var sourceLayoutPath = AssetDatabase.GetAssetPath(layout);
                var destLayoutPath = Path.Combine(UnityConfigHelper.layoutFolderPath, Path.GetFileName(AssetDatabase.GetAssetPath(layout)));
                File.Copy(sourceLayoutPath, destLayoutPath, true);
            }
            InternalEditorUtility.ReloadWindowLayoutMenu();
        }
        void LoadGameViewSizes()
        {
            foreach (var gameViewSizeGroup in config.gameViewSizes)
            {
                foreach (var gameViewSize in gameViewSizeGroup.gameViewSizes)
                {
                    UnityConfigHelper.LoadGameViewSize(gameViewSizeGroup.type, gameViewSize);
                }
            }
        }

        void DrawSnapGUI()
        {

            GeneralDrawGUI("Snap", "Snap Settings", "snap");

        }

        void GeneralDrawGUI(string header, string label, string  propName)
        {
            EditorGUILayout.LabelField(header, UnityConfigHelper.Style.header, GUILayout.Height(32));

            var headerRect = GUILayoutUtility.GetRect(0f, EditorGUIUtility.singleLineHeight, GUILayout.ExpandWidth(true));
           
            if (Event.current.type == EventType.Repaint)
                UnityConfigHelper.Style.lgHeader.Draw(headerRect, false, false, false, false);


            headerRect.xMin += 6f;
            headerRect.xMax -= 6f;
            headerRect.height -= 2f;
            headerRect.y += 1f;

            EditorGUI.LabelField(headerRect, label);
            
            var rect = EditorGUILayout.BeginVertical();
            
            rect.yMax += 3f;
            
            if (Event.current.type == EventType.Repaint)
                UnityConfigHelper.Style.lgBackground.Draw(rect, false, false, false, false);
            
            EditorGUI.indentLevel++;
            
            var preferencesProp = serializedObject.FindProperty(propName);
            EditorGUIUtility.labelWidth += 100;
            
            while (preferencesProp.NextVisible(true))
            {
                if (preferencesProp.propertyPath.StartsWith(propName + "."))
                    EditorGUILayout.PropertyField(preferencesProp);
            }
            
            EditorGUIUtility.labelWidth -= 100;
            
            EditorGUI.indentLevel--;
            
            EditorGUILayout.EndVertical();
        }

        void DrawPreferencesGUI()
        {
            GeneralDrawGUI("Preferences", "Unity Preferences", "preferences");
        }


        void DrawLayoutsGUI()
        {
            EditorGUILayout.LabelField("Layouts", UnityConfigHelper.Style.header, GUILayout.Height(32));

            if (layoutsList == null)
            {
                layoutsList = new ReorderableList(serializedObject, serializedObject.FindProperty("layouts"));
                layoutsList.drawHeaderCallback += position => EditorGUI.LabelField(position, "Layout Assets");

                layoutsList.drawElementCallback += (rect, index, isActive, isFocused) =>
                {
                    var prop = layoutsList.serializedProperty.GetArrayElementAtIndex(index);

                    EditorGUI.BeginChangeCheck();
                    rect.y += 2;
                    rect.height = EditorGUIUtility.singleLineHeight;
                    prop.objectReferenceValue = EditorGUI.ObjectField(rect, prop.objectReferenceValue, typeof(Object), false);

                    if (!EditorGUI.EndChangeCheck())
                        return;

                    if (Path.GetExtension(AssetDatabase.GetAssetPath(prop.objectReferenceValue)) != ".wlt")
                        prop.objectReferenceValue = null;
                };

            }
            layoutsList.DoLayoutList();
        }



        void DrawGameViewSizeGroupGUI()
        {
            EditorGUILayout.LabelField("GameView Size", UnityConfigHelper.Style.header, GUILayout.Height(32));


            var displayNames = config.gameViewSizes.Select(v => v.type.ToString()).ToArray();
            selectedPlatform = GUILayout.SelectionGrid(selectedPlatform, displayNames, displayNames.Count(), EditorStyles.toolbarButton);

            EditorGUILayout.Space();


            var gameViewSizes = serializedObject.FindProperty("gameViewSizes");

            for (var i = 0; i < gameViewSizes.arraySize; i++)
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
                    reorderableList.drawHeaderCallback += position => EditorGUI.LabelField(position, key);
                    reorderableList.onAddCallback += list =>
                    {
                        list.serializedProperty.arraySize++;
                        var newProp = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
                        newProp.FindPropertyRelative("name").stringValue = "New Game Size";
                        newProp.FindPropertyRelative("sizeType").enumValueIndex = 0;
                    };

                    reorderableList.drawElementCallback += (rect, index, isActive, isFocused) =>
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
    }
}