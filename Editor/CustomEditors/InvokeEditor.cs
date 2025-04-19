using UdonSharpEditor;
using UnityEditor;
using UnityEditorInternal;

namespace valenvrc.Common.Editor.Custom
{
    public class InvokeEditor : UnityEditor.Editor
    {

        SerializedProperty methodNameProperty;
        SerializedProperty targetsProperty;

        private ReorderableList reorderableList;

        private void OnEnable(){
            methodNameProperty = serializedObject.FindProperty("methodNames");
            targetsProperty = serializedObject.FindProperty("targets");

            reorderableList = Editor.Utilities.EditorUtilities.CreateReorderableList(
                serializedObject,
                targetsProperty,
                methodNameProperty,
                "Target:",
                "The target object",
                "Method:",
                "The method name to invoke",
                "Target-Method Pairs"
            );
        }

        public override void OnInspectorGUI(){
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
            serializedObject.Update();

            // Draw the reorderable list
            reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}