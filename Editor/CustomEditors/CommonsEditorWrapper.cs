using UnityEditor;

namespace valenvrc.Common.Editor.Custom
{
    [CustomEditor(typeof(GestureInvoke))]
    public class GestureEditorWrapper : GestureEditor{

    }

    [CustomEditor(typeof(Invoke))]
    public class InvokeEditorWrapper : InvokeEditor{

    }
}
