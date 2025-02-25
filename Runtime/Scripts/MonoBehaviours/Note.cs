#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class Note : MonoBehaviour
{
    [SerializeField, HideInInspector, TextArea] string note = "This is an example note used to give general instructions to users of the scene.";
    [SerializeField, HideInInspector] MessageType noteType = MessageType.Info;
}

[CustomEditor(typeof(Note))]
public class NoteEditor : Editor
{
    SerializedProperty note;
    SerializedProperty noteType;

    bool editable = false;
    

    void OnEnable()
    {
        note = serializedObject.FindProperty("note");
        noteType = serializedObject.FindProperty("noteType");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        if (editable)
        {
            EditorGUILayout.PropertyField(note);

            EditorGUILayout.PropertyField(noteType);

            if (GUILayout.Button("Save"))
            {
                editable = false;
            }
        }
        else
        {

            MessageType messageType = (MessageType)noteType.enumValueIndex;

            EditorGUILayout.HelpBox(note.stringValue, messageType);

            if (GUILayout.Button("Edit"))
            {
                editable = true;
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif