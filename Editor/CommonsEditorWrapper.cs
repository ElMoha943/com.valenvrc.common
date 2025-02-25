using UnityEditor;

namespace valenvrc.Common
{
    [CustomEditor(typeof(GestureInvoke))]
    public class GestureEditorWrapper : GestureEditor{

    }

    [CustomEditor(typeof(Invoke))]
    public class InvokeEditorWrapper : InvokeEditor{

    }
}
