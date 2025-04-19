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
            ReorderableList list = new ReorderableList(serializedObject, firstProperty, true, true, true, true);

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
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
                if (index >= 0)
                {
                    firstProperty.DeleteArrayElementAtIndex(index);
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
                secondProperty.MoveArrayElement(oldIndex, newIndex);
            };

            return list;
        }
    }

}