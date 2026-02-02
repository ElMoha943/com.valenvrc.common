using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace valenvrc.Common.Editor.Utilities
{
    public static class EditorUtilities
    {
        public class CustomDialog : EditorWindow{
            private static Texture2D image;
            private static string message;

            public static void ShowDialog(string title, string msg, Texture2D img){
                message = msg;
                image = img;
                CustomDialog window = CreateInstance<CustomDialog>();
                window.titleContent = new GUIContent(title);
                window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 400, 250);
                window.ShowUtility(); // Show as a modal dialog
            }

            void OnGUI(){
                // fixed size
                minSize = new Vector2(500, 200);
                maxSize = new Vector2(500, 200);

                //Style
                GUIStyle messageStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
                messageStyle.richText = true;

                EditorGUILayout.BeginHorizontal();
                if (image){
                    GUILayout.Label(image, GUILayout.Height(128), GUILayout.Width(128));
                }
                GUILayout.Label(message, messageStyle);
                EditorGUILayout.EndHorizontal();

                UdonSharpEditor.UdonSharpGUI.DrawUILine(Color.white, 2, 1);

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("OK", GUILayout.Height(30), GUILayout.Width(100))){
                    Close();
                }
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }
        }

        public static ReorderableList CreateReorderableList(
            SerializedObject serializedObject, 
            SerializedProperty firstProperty, 
            SerializedProperty secondProperty, 
            string firstLabelName = "First:", 
            string firstTooltip = "", 
            string secondLabelName = "Second:", 
            string secondTooltip = "", 
            string header = "Reorderable List")
        {
            // Return null if any required property is null
            if (serializedObject == null || firstProperty == null || secondProperty == null)
                return null;

            ReorderableList list = new ReorderableList(serializedObject, firstProperty, true, true, true, true);

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                // Safety check for array bounds
                if (index >= firstProperty.arraySize || index >= secondProperty.arraySize)
                    return;

                float fieldWidth = rect.width / 2 - 10;
                float padding = 5;

                GUIContent firstLabel = new GUIContent(firstLabelName, firstTooltip);
                GUIContent secondLabel = new GUIContent(secondLabelName, secondTooltip);

                EditorGUI.LabelField(new Rect(rect.x, rect.y, 50, EditorGUIUtility.singleLineHeight), firstLabel);
                EditorGUI.PropertyField(new Rect(rect.x + 50, rect.y, fieldWidth - 50, EditorGUIUtility.singleLineHeight), firstProperty.GetArrayElementAtIndex(index), GUIContent.none);

                EditorGUI.LabelField(new Rect(rect.x + fieldWidth + padding, rect.y, 65, EditorGUIUtility.singleLineHeight), secondLabel);
                EditorGUI.PropertyField(new Rect(rect.x + fieldWidth + 65 + padding, rect.y, fieldWidth - 65, EditorGUIUtility.singleLineHeight), secondProperty.GetArrayElementAtIndex(index), GUIContent.none);
            };

            list.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, header);
            };

            list.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                if (index >= 0 && index < firstProperty.arraySize && index < secondProperty.arraySize)
                {
                    firstProperty.DeleteArrayElementAtIndex(index);
                    if (index < secondProperty.arraySize) // Check again after first deletion
                        secondProperty.DeleteArrayElementAtIndex(index);
                }
            };

            list.onAddCallback = (ReorderableList list) =>
            {
                firstProperty.arraySize++;
                secondProperty.arraySize++;
            };

            list.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
            {
                if (oldIndex >= 0 && oldIndex < secondProperty.arraySize && newIndex >= 0 && newIndex < secondProperty.arraySize)
                    secondProperty.MoveArrayElement(oldIndex, newIndex);
            };

            return list;
        }

        public static void DrawDefaultInspector(SerializedObject serializedObject)
        {
            SerializedProperty iterator = serializedObject.GetIterator();
            bool enterChildren = true;
            while (iterator.NextVisible(enterChildren))
            {
                if (iterator.name == "m_Script" || iterator.name == "m_EditorData")
                    continue;
                EditorGUILayout.PropertyField(iterator, true);
                enterChildren = false;
            }
        }
    }

}