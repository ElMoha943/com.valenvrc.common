using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System;

namespace valenvrc.Common.Editor.Utilities
{
    public class CollapsibleReorderableList
    {
        public bool expanded = true;

        private readonly ReorderableList _list;
        private readonly GUIContent _headerLabel;
        private readonly GUIContent _dropdownLabel;
        private readonly Action<GenericMenu> _buildMenu;
        private readonly bool _boldHeader;
        private GUIStyle _foldoutStyle;

        public CollapsibleReorderableList(ReorderableList list, string header, string dropdownButtonLabel = null, Action<GenericMenu> buildMenu = null, bool bold = false)
        {
            _list = list;
            _headerLabel = new GUIContent(header);
            _dropdownLabel = string.IsNullOrEmpty(dropdownButtonLabel) ? null : new GUIContent(dropdownButtonLabel);
            _buildMenu = buildMenu;
            _boldHeader = bold;
            _list.drawHeaderCallback = DrawHeader;
        }

        public int Count => _list.count;

        private GUIStyle GetFoldoutStyle()
        {
            if (_foldoutStyle == null)
            {
                _foldoutStyle = new GUIStyle(EditorStyles.foldout);
                if (_boldHeader)
                    _foldoutStyle.fontStyle = FontStyle.Bold;
            }
            return _foldoutStyle;
        }

        private void DrawHeader(Rect rect)
        {
            bool showDropdown = _dropdownLabel != null && _buildMenu != null;

            GUIStyle style = GetFoldoutStyle();

            if (showDropdown)
            {
                const float buttonWidth = 95f;
                const float spacing = 4f;
                Rect buttonRect = new Rect(rect.xMax - buttonWidth, rect.y + 1f, buttonWidth, EditorGUIUtility.singleLineHeight);
                Rect foldoutRect = new Rect(rect.x, rect.y, rect.width - buttonWidth - spacing, rect.height);
                expanded = EditorGUI.Foldout(foldoutRect, expanded, _headerLabel, true, style);
                if (GUI.Button(buttonRect, _dropdownLabel, EditorStyles.miniButton))
                {
                    GenericMenu menu = new GenericMenu();
                    _buildMenu(menu);
                    menu.DropDown(buttonRect);
                }
            }
            else
            {
                expanded = EditorGUI.Foldout(rect, expanded, _headerLabel, true, style);
            }
        }

        public void DoLayoutList()
        {
            float indent = EditorGUI.indentLevel * 15f;
            bool hasIndent = indent > 0f;

            if (hasIndent)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(indent);
                EditorGUILayout.BeginVertical();
            }

            if (expanded)
            {
                _list.DoLayoutList();
            }
            else
            {
                Rect rect = GUILayoutUtility.GetRect(0f, 20f, GUILayout.ExpandWidth(true));
                if (Event.current.type == EventType.Repaint)
                    GUI.Label(rect, GUIContent.none, "RL Header");
                DrawHeader(rect);
            }

            if (hasIndent)
            {
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
