using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace valenvrc.Common
{
    public class DefineManager
    {
      [MenuItem("Valenvrc/Reload Defines", false, 1)]
      [DidReloadScripts]
      private static void OnScriptReload() => DefineManager.UpdateDefines();

      private static void UpdateDefines()
      {
        Debug.Log((object) "[<color=orange>ValenDefineManager</color>] >> Reloading valen defines");
        List<string> stringList1 = new List<string>()
        {
          "VALENVRC_COMMON"
        };
        List<string> stringList2 = new List<string>();
        foreach (KeyValuePair<string, string> asset in ValenAssets.AssetList)
        {
          try
          {
            if (Assembly.Load(asset.Value) != (Assembly) null)
              stringList1.Add("VALENVRC_" + asset.Key.ToUpper());
          }
          catch
          {
            stringList2.Add("VALENVRC_" + asset.Key.ToUpper());
          }
        }
        BuildTargetGroup buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        if (buildTargetGroup == null)
        {
          Debug.LogWarning((object) "[<color=orange>ValenDefineManager</color>] >> Unknown build target group. Skipping define update.");
        }
        else
        {
          if (stringList1.Count > 0)
            DefineManager.AddDefinesIfMissing(buildTargetGroup, stringList1.ToArray());
          if (stringList2.Count <= 0)
            return;
          DefineManager.RemoveDefinesIfPresent(buildTargetGroup, stringList2.ToArray());
        }
      }

      private static void AddDefinesIfMissing(BuildTargetGroup buildTarget, params string[] newDefines)
      {
        bool flag = false;
        string str1 = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget) ?? string.Empty;
        HashSet<string> source = new HashSet<string>();
        if (str1.Length > 0)
          source = new HashSet<string>((IEnumerable<string>) str1.Split(';', StringSplitOptions.None));
        foreach (string newDefine in newDefines)
        {
          if (source.Add(newDefine))
            flag = true;
        }
        if (flag)
        {
          string str2 = string.Join(";", source.ToArray<string>());
          PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, str2);
          Debug.LogFormat("[<color=orange>ValenDefineManager</color>] >> Set Scripting Define Symbols for selected build target ({0}) to: {1}", new object[2]
          {
            (object) buildTarget.ToString(),
            (object) str2
          });
        }
        else
          Debug.Log((object) "[<color=orange>ValenDefineManager</color>] >> No changes made. Defines were already present.");
      }

      private static void RemoveDefinesIfPresent(
        BuildTargetGroup buildTarget,
        params string[] definesToRemove)
      {
        bool flag = false;
        string str1 = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget) ?? string.Empty;
        HashSet<string> source = new HashSet<string>();
        if (str1.Length > 0)
          source = new HashSet<string>((IEnumerable<string>) str1.Split(';', StringSplitOptions.None));
        foreach (string str2 in definesToRemove)
        {
          if (source.Remove(str2))
            flag = true;
        }
        if (flag)
        {
          string str3 = string.Join(";", source.ToArray<string>());
          PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, str3);
          Debug.LogFormat("[<color=orange>ValenDefineManager</color>] >> Removed Scripting Define Symbols for selected build target ({0}): {1}", new object[2]
          {
            (object) buildTarget.ToString(),
            (object) string.Join(", ", definesToRemove)
          });
        }
        else
          Debug.Log((object) "[<color=orange>ValenDefineManager</color>] >> No changes made. Defines to remove were not found.");
      }
    }
}
