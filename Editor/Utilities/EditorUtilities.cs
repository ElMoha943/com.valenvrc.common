using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

namespace valenvrc.Common.Editor.Utilities
{
    public static class EditorUtilities
    {
        public struct ReorderableListColumn
        {
            public SerializedProperty Property;
            public string Label;
            public string Tooltip;

            public ReorderableListColumn(SerializedProperty property, string label = null, string tooltip = "")
            {
                Property = property;
                Label = label;
                Tooltip = tooltip;
            }
        }

        public static CollapsibleReorderableList CreateReorderableList(
            SerializedObject serializedObject,
            ReorderableListColumn[] columns,
            string header = "Reorderable List",
            string dropdownButtonLabel = null,
            Action<GenericMenu> buildHeaderDropdownMenu = null,
            bool boldHeader = false)
        {
            // Return null if any required property is null
            if (serializedObject == null || columns == null || columns.Length == 0)
                return null;

            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Property == null)
                    return null;
            }

            SerializedProperty firstProperty = columns[0].Property;
            int targetSize = 0;
            for (int i = 0; i < columns.Length; i++)
            {
                if (columns[i].Property.arraySize > targetSize)
                    targetSize = columns[i].Property.arraySize;
            }

            for (int i = 0; i < columns.Length; i++)
            {
                columns[i].Property.arraySize = targetSize;
            }

            ReorderableList list = new ReorderableList(serializedObject, firstProperty, true, true, true, true);

            GUIContent[] columnLabels = new GUIContent[columns.Length];
            float[] labelWidths = new float[columns.Length];
            for (int i = 0; i < columns.Length; i++)
            {
                string columnLabel = string.IsNullOrEmpty(columns[i].Label) ? columns[i].Property.displayName + ":" : columns[i].Label;
                columnLabels[i] = new GUIContent(columnLabel, columns[i].Tooltip);
                labelWidths[i] = Mathf.Clamp(EditorStyles.label.CalcSize(columnLabels[i]).x + 4f, 36f, 120f);
            }

            list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                for (int i = 0; i < columns.Length; i++)
                {
                    if (index >= columns[i].Property.arraySize)
                        return;
                }

                const float spacing = 6f;
                float totalSpacing = spacing * (columns.Length - 1);
                float columnWidth = (rect.width - totalSpacing) / columns.Length;

                for (int i = 0; i < columns.Length; i++)
                {
                    float x = rect.x + i * (columnWidth + spacing);
                    float labelWidth = labelWidths[i];
                    float fieldWidth = Mathf.Max(0f, columnWidth - labelWidth);
                    Rect labelRect = new Rect(x, rect.y, labelWidth, EditorGUIUtility.singleLineHeight);
                    Rect fieldRect = new Rect(x + labelWidth, rect.y, fieldWidth, EditorGUIUtility.singleLineHeight);

                    EditorGUI.LabelField(labelRect, columnLabels[i]);
                    EditorGUI.PropertyField(fieldRect, columns[i].Property.GetArrayElementAtIndex(index), GUIContent.none);
                }
            };

            list.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                for (int i = 0; i < columns.Length; i++)
                {
                    if (index >= 0 && index < columns[i].Property.arraySize)
                        columns[i].Property.DeleteArrayElementAtIndex(index);
                }
            };

            list.onAddCallback = (ReorderableList list) =>
            {
                int newSize = firstProperty.arraySize + 1;
                for (int i = 0; i < columns.Length; i++)
                {
                    columns[i].Property.arraySize = newSize;
                }
            };

            list.onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
            {
                for (int i = 1; i < columns.Length; i++)
                {
                    SerializedProperty property = columns[i].Property;
                    if (oldIndex >= 0 && oldIndex < property.arraySize && newIndex >= 0 && newIndex < property.arraySize)
                        property.MoveArrayElement(oldIndex, newIndex);
                }
            };

            return new CollapsibleReorderableList(list, header, dropdownButtonLabel, buildHeaderDropdownMenu, boldHeader);
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

        private static int _sectionDepth = 0;
        private static readonly System.Collections.Generic.Stack<bool> _sectionStack = new System.Collections.Generic.Stack<bool>();

        private static readonly Color[] _headerColorsDark =
        {
            new Color(0.27f, 0.27f, 0.27f),
            new Color(0.22f, 0.22f, 0.22f),
            new Color(0.17f, 0.17f, 0.17f),
            new Color(0.13f, 0.13f, 0.13f),
        };

        private static readonly Color[] _headerColorsLight =
        {
            new Color(0.73f, 0.73f, 0.73f),
            new Color(0.66f, 0.66f, 0.66f),
            new Color(0.59f, 0.59f, 0.59f),
            new Color(0.52f, 0.52f, 0.52f),
        };

        private static readonly Color[] _accentColors =
        {
            new Color(0.24f, 0.59f, 0.88f),
            new Color(0.24f, 0.82f, 0.55f),
            new Color(0.94f, 0.73f, 0.24f),
            new Color(0.88f, 0.40f, 0.24f),
        };

        private static GUIStyle _sectionHeaderStyle;
        private static GUIStyle _sectionArrowStyle;
        private static GUIStyle _sectionContentStyle;

        public static bool BeginFoldoutSection(bool expanded, string header)
        {
            if (_sectionHeaderStyle == null)
            {
                _sectionHeaderStyle = new GUIStyle(EditorStyles.boldLabel);
                _sectionHeaderStyle.fontSize = 11;
            }

            if (_sectionArrowStyle == null)
            {
                _sectionArrowStyle = new GUIStyle(EditorStyles.label);
                _sectionArrowStyle.alignment = TextAnchor.MiddleCenter;
                _sectionArrowStyle.fontSize = 9;
            }

            if (_sectionContentStyle == null)
            {
                _sectionContentStyle = new GUIStyle("helpBox");
                _sectionContentStyle.padding = new RectOffset(8, 8, 6, 8);
                _sectionContentStyle.margin = new RectOffset(0, 0, 0, 0);
            }

            int depth = Mathf.Clamp(_sectionDepth, 0, 3);
            Color headerBg = EditorGUIUtility.isProSkin ? _headerColorsDark[depth] : _headerColorsLight[depth];
            Color accent = _accentColors[depth % _accentColors.Length];
            Color textColor = EditorGUIUtility.isProSkin ? new Color(0.9f, 0.9f, 0.9f) : Color.black;

            _sectionHeaderStyle.normal.textColor = textColor;
            _sectionArrowStyle.normal.textColor = textColor;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.Space(1f);

            Rect headerRect = GUILayoutUtility.GetRect(0f, 24f, GUILayout.ExpandWidth(true));

            if (Event.current.type == EventType.Repaint)
            {
                EditorGUI.DrawRect(headerRect, headerBg);
                EditorGUI.DrawRect(new Rect(headerRect.x, headerRect.y, 4f, headerRect.height), accent);

                string arrow = expanded ? "▼" : "▶";
                GUI.Label(new Rect(headerRect.x + 6f, headerRect.y + 3f, 14f, 18f), arrow, _sectionArrowStyle);
                GUI.Label(new Rect(headerRect.x + 22f, headerRect.y + 3f, headerRect.width - 26f, 18f), header, _sectionHeaderStyle);
            }

            if (Event.current.type == EventType.MouseDown && headerRect.Contains(Event.current.mousePosition))
            {
                expanded = !expanded;
                Event.current.Use();
                GUI.changed = true;
            }

            _sectionStack.Push(expanded);
            _sectionDepth++;

            if (expanded)
                EditorGUILayout.BeginVertical(_sectionContentStyle);

            return expanded;
        }

        public static void EndFoldoutSection()
        {
            bool expanded = _sectionStack.Count > 0 ? _sectionStack.Pop() : false;
            _sectionDepth = Mathf.Max(0, _sectionDepth - 1);

            if (expanded)
                EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space(1f);
        }

        
    }

}