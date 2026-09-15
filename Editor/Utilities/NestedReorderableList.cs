using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace valenvrc.Common.Editor.Utilities
{
    /// <summary>
    /// Reflection-backed access to a serialized field that Unity cannot represent as a SerializedProperty,
    /// such as a jagged array used by UdonSharp.
    /// </summary>
    public sealed class SerializedField<T>
    {
        private readonly SerializedObject serializedObject;
        private readonly FieldInfo field;

        public SerializedField(SerializedObject serializedObject, string fieldName)
        {
            if (serializedObject == null)
                throw new ArgumentNullException(nameof(serializedObject));
            if (string.IsNullOrEmpty(fieldName))
                throw new ArgumentException("A field name is required.", nameof(fieldName));
            if (serializedObject.isEditingMultipleObjects)
                throw new NotSupportedException("SerializedField does not support editing multiple objects at once.");

            this.serializedObject = serializedObject;
            field = FindField(serializedObject.targetObject.GetType(), fieldName);

            if (field == null)
                throw new ArgumentException($"Field '{fieldName}' was not found on {serializedObject.targetObject.GetType().Name}.", nameof(fieldName));
            if (!typeof(T).IsAssignableFrom(field.FieldType))
                throw new ArgumentException($"Field '{fieldName}' is {field.FieldType.Name}, not {typeof(T).Name}.", nameof(fieldName));
        }

        public UnityEngine.Object Target => serializedObject.targetObject;

        public T Value
        {
            get => (T)field.GetValue(Target);
            set => field.SetValue(Target, value);
        }

        private static FieldInfo FindField(Type type, string fieldName)
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            while (type != null)
            {
                FieldInfo result = type.GetField(fieldName, flags);
                if (result != null)
                    return result;
                type = type.BaseType;
            }

            return null;
        }
    }

    /// <summary>
    /// Draws a two-level, reorderable object array with add and remove controls at both levels.
    /// </summary>
    public sealed class NestedReorderableList<T> : IDisposable where T : UnityEngine.Object
    {
        private const float Spacing = 4f;

        private readonly SerializedField<T[][]> serializedField;
        private readonly string columnName;
        private readonly string indexName;
        private ReorderableList columnList;
        private ReorderableList[] indexLists;

        public NestedReorderableList(SerializedField<T[][]> serializedField, string columnName, string indexName)
        {
            this.serializedField = serializedField ?? throw new ArgumentNullException(nameof(serializedField));
            this.columnName = string.IsNullOrWhiteSpace(columnName) ? "Column" : columnName;
            this.indexName = string.IsNullOrWhiteSpace(indexName) ? "Item" : indexName;

            EnsureArrays();
            BuildLists();
            Undo.undoRedoPerformed += RebuildAfterUndoRedo;
        }

        public void Dispose()
        {
            Undo.undoRedoPerformed -= RebuildAfterUndoRedo;
        }

        public void DoLayoutList()
        {
            if (serializedField.Target == null)
                return;

            // Recording before drawing also captures drag-to-reorder operations.
            Undo.RecordObject(serializedField.Target, $"Edit {Pluralize(columnName)}");
            columnList.DoLayoutList();
        }

        private void EnsureArrays()
        {
            T[][] values = serializedField.Value;
            if (values == null)
                values = new T[0][];

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i] == null)
                    values[i] = new T[0];
            }

            serializedField.Value = values;
        }

        private void BuildLists()
        {
            T[][] values = serializedField.Value;
            indexLists = new ReorderableList[values.Length];
            for (int i = 0; i < indexLists.Length; i++)
                indexLists[i] = BuildIndexList(i);

            columnList = new ReorderableList(values, typeof(T[]), true, true, true, true);
            columnList.drawHeaderCallback = rect => EditorGUI.LabelField(rect, Pluralize(columnName));
            columnList.elementHeightCallback = columnIndex =>
            {
                if (columnIndex < 0 || columnIndex >= indexLists.Length)
                    return EditorGUIUtility.singleLineHeight;

                return EditorGUIUtility.singleLineHeight + Spacing + indexLists[columnIndex].GetHeight() + Spacing;
            };
            columnList.drawElementCallback = (rect, columnIndex, isActive, isFocused) =>
            {
                if (columnIndex < 0 || columnIndex >= indexLists.Length)
                    return;

                Rect labelRect = new Rect(rect.x, rect.y + 1f, rect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(labelRect, $"{columnName} {columnIndex + 1}", EditorStyles.boldLabel);

                Rect itemsRect = new Rect(
                    rect.x,
                    labelRect.yMax + Spacing,
                    rect.width,
                    indexLists[columnIndex].GetHeight());
                indexLists[columnIndex].DoList(itemsRect);
            };
            columnList.onAddCallback = list =>
            {
                T[][] current = serializedField.Value;
                int oldLength = current.Length;
                Array.Resize(ref current, oldLength + 1);
                current[oldLength] = new T[0];
                serializedField.Value = current;
                BuildLists();
                columnList.index = oldLength;
                MarkChanged();
            };
            columnList.onRemoveCallback = list =>
            {
                T[][] current = serializedField.Value;
                if (list.index < 0 || list.index >= current.Length)
                    return;

                serializedField.Value = RemoveAt(current, list.index);
                BuildLists();
                MarkChanged();
            };
            columnList.onCanRemoveCallback = list => serializedField.Value.Length > 0;
            columnList.onReorderCallback = list =>
            {
                serializedField.Value = (T[][])list.list;
                BuildLists();
                MarkChanged();
            };
        }

        private ReorderableList BuildIndexList(int columnIndex)
        {
            ReorderableList list = new ReorderableList(serializedField.Value[columnIndex], typeof(T), true, true, true, true);
            list.drawHeaderCallback = rect => EditorGUI.LabelField(rect, Pluralize(indexName));
            list.elementHeight = EditorGUIUtility.singleLineHeight + 2f;
            list.drawElementCallback = (rect, itemIndex, isActive, isFocused) =>
            {
                T[][] current = serializedField.Value;
                if (columnIndex >= current.Length || itemIndex >= current[columnIndex].Length)
                    return;

                rect.y += 1f;
                rect.height = EditorGUIUtility.singleLineHeight;
                EditorGUI.BeginChangeCheck();
                T value = (T)EditorGUI.ObjectField(
                    rect,
                    $"{indexName} {itemIndex + 1}",
                    current[columnIndex][itemIndex],
                    typeof(T),
                    true);
                if (EditorGUI.EndChangeCheck())
                {
                    current[columnIndex][itemIndex] = value;
                    serializedField.Value = current;
                    MarkChanged();
                }
            };
            list.onAddCallback = innerList =>
            {
                T[][] current = serializedField.Value;
                T[] items = current[columnIndex];
                Array.Resize(ref items, items.Length + 1);
                current[columnIndex] = items;
                serializedField.Value = current;
                innerList.list = items;
                innerList.index = items.Length - 1;
                MarkChanged();
            };
            list.onRemoveCallback = innerList =>
            {
                T[][] current = serializedField.Value;
                if (innerList.index < 0 || innerList.index >= current[columnIndex].Length)
                    return;

                T[] items = RemoveAt(current[columnIndex], innerList.index);
                current[columnIndex] = items;
                serializedField.Value = current;
                innerList.list = items;
                MarkChanged();
            };
            list.onCanRemoveCallback = innerList => serializedField.Value[columnIndex].Length > 0;
            list.onReorderCallback = innerList =>
            {
                T[][] current = serializedField.Value;
                current[columnIndex] = (T[])innerList.list;
                serializedField.Value = current;
                MarkChanged();
            };
            return list;
        }

        private void MarkChanged()
        {
            EditorUtility.SetDirty(serializedField.Target);
        }

        private void RebuildAfterUndoRedo()
        {
            if (serializedField.Target == null)
                return;

            EnsureArrays();
            BuildLists();
        }

        private static TItem[] RemoveAt<TItem>(TItem[] source, int index)
        {
            TItem[] result = new TItem[source.Length - 1];
            if (index > 0)
                Array.Copy(source, 0, result, 0, index);
            if (index < source.Length - 1)
                Array.Copy(source, index + 1, result, index, source.Length - index - 1);
            return result;
        }

        private static string Pluralize(string name)
        {
            return name.EndsWith("s", StringComparison.OrdinalIgnoreCase) ? name : name + "s";
        }
    }
}
