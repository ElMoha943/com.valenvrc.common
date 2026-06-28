using UnityEditor;
using UnityEngine;

namespace valenvrc.Common.Editor.Utilities
{
    public class CustomDialog : EditorWindow
    {
        private static Texture2D image;
        private static string message;

        public static void ShowDialog(string title, string msg, Texture2D img)
        {
            message = msg;
            image = img;
            CustomDialog window = CreateInstance<CustomDialog>();
            window.titleContent = new GUIContent(title);
            window.position = new Rect(Screen.width / 2f, Screen.height / 2f, 400, 250);
            window.ShowUtility();
        }

        void OnGUI()
        {
            minSize = new Vector2(500, 200);
            maxSize = new Vector2(500, 200);

            GUIStyle messageStyle = new GUIStyle(EditorStyles.wordWrappedLabel);
            messageStyle.richText = true;

            EditorGUILayout.BeginHorizontal();
            if (image)
                GUILayout.Label(image, GUILayout.Height(128), GUILayout.Width(128));
            GUILayout.Label(message, messageStyle);
            EditorGUILayout.EndHorizontal();

            UdonSharpEditor.UdonSharpGUI.DrawUILine(Color.white, 2, 1);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("OK", GUILayout.Height(30), GUILayout.Width(100)))
                Close();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}
