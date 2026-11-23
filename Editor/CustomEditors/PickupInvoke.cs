using UdonSharpEditor;
using UnityEditor;

namespace valenvrc.Common.Editor.Custom
{
    [CustomEditor(typeof(PickupInvoke))]
    public class PickupInvokeEditor : UnityEditor.Editor
    {
        SerializedObject serializedObj;

        SerializedProperty typeProperty;

        SerializedProperty methodNameProperty;
        SerializedProperty targetProperty;

        SerializedProperty toggleObjectsProperty;

        SerializedProperty audioSourceProperty;

        SerializedProperty particleProperty;

        SerializedProperty animatorProperty;
        SerializedProperty triggerNameProperty;

        void OnEnable(){
            serializedObj = new SerializedObject(target);
            typeProperty = serializedObj.FindProperty("type");

            methodNameProperty = serializedObj.FindProperty("methodName");
            targetProperty = serializedObj.FindProperty("target");

            toggleObjectsProperty = serializedObj.FindProperty("toggleObjects");

            audioSourceProperty = serializedObj.FindProperty("audioSource");

            particleProperty = serializedObj.FindProperty("particle");

            animatorProperty = serializedObj.FindProperty("animator");
            triggerNameProperty = serializedObj.FindProperty("triggerName");
        }

        public override void OnInspectorGUI(){
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(typeProperty);
            switch (((PickupInvoke)target).type)
            {
                case PickupInvokeType.Invoke:
                    EditorGUILayout.PropertyField(methodNameProperty);
                    EditorGUILayout.PropertyField(targetProperty);
                    break;
                case PickupInvokeType.Toggle:
                    EditorGUILayout.PropertyField(toggleObjectsProperty, true);
                    break;
                case PickupInvokeType.Sound:
                    EditorGUILayout.PropertyField(audioSourceProperty);
                    break;
                case PickupInvokeType.Particle:
                    EditorGUILayout.PropertyField(particleProperty);
                    break;
                case PickupInvokeType.Animation:
                    EditorGUILayout.PropertyField(animatorProperty);
                    EditorGUILayout.PropertyField(triggerNameProperty);
                    break;
            }
            if(EditorGUI.EndChangeCheck()){
                serializedObj.ApplyModifiedProperties();
            }
        }
    }
}