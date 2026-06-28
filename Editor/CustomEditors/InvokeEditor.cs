using UdonSharpEditor;
using UnityEditor;
using valenvrc.Common.Editor.Utilities;

namespace valenvrc.Common.Editor.Custom
{
    [CustomEditor(typeof(Invoke))]
    public class InvokeEditor : UnityEditor.Editor
    {

        SerializedProperty methodNameProperty;
        SerializedProperty targetsProperty;

        private CollapsibleReorderableList reorderableList;

        private void TryBuildList()
        {
            methodNameProperty = serializedObject.FindProperty("methodNames");
            targetsProperty = serializedObject.FindProperty("targets");

            if (methodNameProperty == null || targetsProperty == null)
                return;

            Utilities.EditorUtilities.ReorderableListColumn[] columns = new Utilities.EditorUtilities.ReorderableListColumn[2];
            columns[0] = new Utilities.EditorUtilities.ReorderableListColumn(targetsProperty, "Target:", "The target object");
            columns[1] = new Utilities.EditorUtilities.ReorderableListColumn(methodNameProperty, "Method:", "The method name to invoke");

            reorderableList = Utilities.EditorUtilities.CreateReorderableList(
                serializedObject,
                columns,
                "Target-Method Pairs"
            );
        }

        private void OnEnable(){
            TryBuildList();
        }

        public override void OnInspectorGUI(){
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
            serializedObject.Update();

            if (reorderableList == null)
                TryBuildList();

            if (reorderableList != null)
                reorderableList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }
    }
}