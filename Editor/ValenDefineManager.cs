#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using System.IO;
using UnityEditor.Callbacks;

namespace valenvrc.Common{
    public class ValenDefineManager{

        [DidReloadScripts]
        static void OnScriptReload(){
            UpdateDefines();
        }

        static void UpdateDefines(){
            Debug.Log("[ValenDefineManager] >> Reloading valen defines");
            List<string> DefinesToAdd = new List<string> { "VALENVRC_COMMON" };
            List<string> DefinesToRemove = new List<string>();

            if(File.Exists("Packages/com.valenvrc.admin_tablet/package.json")){
                DefinesToAdd.Add("VALENVRC_ADMINTABLET");
            }
            else{
                DefinesToRemove.Add("VALENVRC_ADMINTABLET");
            }

            if(File.Exists("Packages/com.valenvrc.security_keypad/package.json")){
                DefinesToAdd.Add("VALENVRC_KEYPAD");
            }
            else{
                DefinesToRemove.Add("VALENVRC_KEYPAD");
            }

            BuildTargetGroup buildTarget = EditorUserBuildSettings.selectedBuildTargetGroup;
            if(buildTarget == BuildTargetGroup.Unknown){
                Debug.LogWarning("[ValenDefineManager] >> Unknown build target group. Skipping define update.");
                return;
            }

            if(DefinesToAdd.Count > 0) AddDefinesIfMissing(buildTarget, DefinesToAdd.ToArray());
            if (DefinesToRemove.Count > 0) RemoveDefinesIfPresent(buildTarget, DefinesToRemove.ToArray());
        }

        static void AddDefinesIfMissing(BuildTargetGroup buildTarget, params string[] newDefines){
            bool definesChanged = false;
            string existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget) ?? string.Empty;
            HashSet<string> defineSet = new HashSet<string>();

            if (existingDefines.Length > 0)
                defineSet = new HashSet<string>(existingDefines.Split(';'));

            foreach (string newDefine in newDefines)
                if (defineSet.Add(newDefine))
                    definesChanged = true;

            if (definesChanged){
                string finalDefineString = string.Join(";", defineSet.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, finalDefineString);
                Debug.LogFormat("[ValenDefineManager] >> Set Scripting Define Symbols for selected build target ({0}) to: {1}", buildTarget.ToString(), finalDefineString);
            }
            else{
                Debug.Log("[ValenDefineManager] >> No changes made. Defines were already present.");
            }
        }

        static void RemoveDefinesIfPresent(BuildTargetGroup buildTarget, params string[] definesToRemove) {
            bool definesChanged = false;
            string existingDefines = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget) ?? string.Empty;
            HashSet<string> defineSet = new HashSet<string>();

            if (existingDefines.Length > 0)
                defineSet = new HashSet<string>(existingDefines.Split(';'));

            foreach (string defineToRemove in definesToRemove)
                if (defineSet.Remove(defineToRemove))
                    definesChanged = true;

            if (definesChanged) {
                string finalDefineString = string.Join(";", defineSet.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, finalDefineString);
                Debug.LogFormat("[ValenDefineManager] >> Removed Scripting Define Symbols for selected build target ({0}): {1}", buildTarget.ToString(), string.Join(", ", definesToRemove));
            } else {
                Debug.Log("[ValenDefineManager] >> No changes made. Defines to remove were not found.");
            }
        }

    }
}
#endif