using UdonSharpEditor;
using UnityEditor;

namespace valenvrc.Common.Editor.Custom
{
    public class GestureEditor : UnityEditor.Editor
    {
        SerializedObject serializedObj;

        SerializedProperty requireTriggerPressedProperty;
        SerializedProperty gestureTypeProperty;
        SerializedProperty gestureActionProperty;
        SerializedProperty targetPositionProperty;
        SerializedProperty RespawnKeyProperty;
        SerializedProperty timesProperty;
        SerializedProperty gracePeriodProperty;

        SerializedProperty targetObjectProperty;
        SerializedProperty tpointProperty;

        void OnEnable(){
            serializedObj = new SerializedObject(target);

            requireTriggerPressedProperty = serializedObj.FindProperty("RequireTriggerPressed");

            gestureTypeProperty = serializedObj.FindProperty("gestureType");
            gestureActionProperty = serializedObj.FindProperty("gestureAction");
            targetPositionProperty = serializedObj.FindProperty("targetPosition");
            RespawnKeyProperty = serializedObj.FindProperty("RespawnKey");
            timesProperty = serializedObj.FindProperty("times");
            gracePeriodProperty = serializedObj.FindProperty("gracePeriod");

            targetObjectProperty = serializedObj.FindProperty("targetObject");
            tpointProperty = serializedObj.FindProperty("tpoint");
        }

        public override void OnInspectorGUI(){
            UdonSharpGUI.DrawDefaultUdonSharpBehaviourHeader(target);
            serializedObj.Update();

            EditorGUILayout.PropertyField(requireTriggerPressedProperty);

            EditorGUILayout.PropertyField(gestureTypeProperty);
            if(gestureTypeProperty.enumValueIndex > 0){ // Respawn Joystick Gesture
                EditorGUILayout.PropertyField(timesProperty);
                if(gestureTypeProperty.enumValueIndex > 2){ // Times Gesture
                    EditorGUILayout.PropertyField(gracePeriodProperty);
                }
            }
            else{ // Respawn Key
                EditorGUILayout.PropertyField(RespawnKeyProperty);
            }

            EditorGUILayout.PropertyField(gestureActionProperty);

            //Show Object
            if(gestureActionProperty.enumValueIndex == 0 || gestureActionProperty.enumValueIndex == 1){ // Teleport/Toggle Object
                EditorGUILayout.PropertyField(targetObjectProperty);
            }

            //Show Position type
            if(gestureActionProperty.enumValueIndex == 1){ // Teleport Object
                EditorGUILayout.PropertyField(targetPositionProperty);
            }

            //Show Position
            if(targetPositionProperty.enumValueIndex == 3 || gestureActionProperty.enumValueIndex == 2){ // Custom Position or Teleport Self
                EditorGUILayout.PropertyField(tpointProperty);
            }

            serializedObj.ApplyModifiedProperties();
        }
    }
}
