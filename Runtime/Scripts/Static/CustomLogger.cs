using UdonSharp;
using UnityEngine;

namespace valenvrc.Common
{
    public class CustomLogger : UdonSharpBehaviour
    {
        public static void Log(string prefix, string message)
        {
            Log(prefix, message, DebugMode.Info);
        }

        public static void Log(string prefix, string message, DebugMode logType)
        {
            switch (logType)
            {
                case DebugMode.Debug:
                case DebugMode.Info:
                    Debug.Log(prefix + message);
                    break;
                case DebugMode.Warning:
                    Debug.LogWarning(prefix + message);
                    break;
                case DebugMode.Error:
                    Debug.LogError(prefix + message);
                    break;
            }
        }
    }
}