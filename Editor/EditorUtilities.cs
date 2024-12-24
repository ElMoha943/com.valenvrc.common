using UnityEditor;
using UnityEngine;

namespace valenvrc.Common{
    public class EditorUtilities : Editor
    {
        [System.Serializable]
        public class PackageData{
            public string version;
        }

        public static void DrawGradientText(Rect rect, string text, Gradient gradient, GUIStyle style){
            // Calculate the total text width
            Vector2 textSize = style.CalcSize(new GUIContent(text));
            float totalTextWidth = textSize.x;

            // Calculate character width
            float charWidth = totalTextWidth / text.Length;
            

            // Calculate the starting x-position to center the text
            float startX = rect.x + (rect.width - totalTextWidth) / 2;

            // Draw each character with its corresponding gradient color
            for (int i = 0; i < text.Length; i++)
            {
                float t = (float)i / (text.Length - 1); // Gradient position (0 to 1)
                Color color = gradient.Evaluate(t);    // Get the corresponding gradient color

                // Set the color for the current character
                style.normal.textColor = color;

                //add 1 offset if the character is a D
                if(text[i] == 'M' || text[i] == 'm' || text[i] == 'U' || text[i] == 'u'){
                    startX += 1.5f;
                }

                // Calculate the position of the character
                Rect charRect = new Rect(startX + i * charWidth, rect.y, charWidth, rect.height);

                // Draw the character
                GUI.Label(charRect, text[i].ToString(), style);
            }
        }

        


    }
}


