using UnityEngine;
using UnityEditor;

namespace Kyusyukeigo.Helper
{
    [CustomPropertyDrawer(typeof(GameViewSize))]
    public class GameViewSizeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var nameProp = property.FindPropertyRelative("name");
            var sizeTypeProp = property.FindPropertyRelative("sizeType");
            var widthProp = property.FindPropertyRelative("width");
            var heightProp = property.FindPropertyRelative("height");



            position.height = EditorGUIUtility.singleLineHeight;

            EditorGUI.BeginProperty(position, label, property);
            position.y += 4;
            EditorGUI.PropertyField(position, nameProp, new GUIContent("Label"));
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, sizeTypeProp, new GUIContent("Type"));
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, widthProp);
            position.y += EditorGUIUtility.singleLineHeight;
            EditorGUI.PropertyField(position, heightProp);
            position.y += EditorGUIUtility.singleLineHeight + 4;
            position.x -= 18;
            position.width += 23;
            position.height = 2.2f;
            EditorGUI.LabelField(position, "", new GUIStyle("box"));
            EditorGUI.EndProperty();

        }
    }
}