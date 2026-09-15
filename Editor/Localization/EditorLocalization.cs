using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace valenvrc.Common.Editor.Localization
{
    /// <summary>
    /// Provides reusable, editor-only localization with shared Valen Commons strings,
    /// per-asset string tables, and English fallback.
    /// </summary>
    public sealed class EditorLocalization
    {
        private const string LanguagePreferenceKey = "valenvrc.Common.EditorLanguage";
        private const string CommonLanguageTablePath = "Packages/com.valenvrc.common/Editor/Localization/Languages/{0}.txt";
        private const string EnglishLanguageCode = "en";

        private static readonly string[] LanguageCodes = { EnglishLanguageCode, "es" };
        private static readonly string[] LanguageNames = { "English", "Español" };
        private static readonly Dictionary<string, Dictionary<string, string>> CommonTables = new Dictionary<string, Dictionary<string, string>>();
        private static readonly HashSet<string> ReportedMissingKeys = new HashSet<string>();

        private readonly string assetLanguageTablePath;
        private readonly Dictionary<string, Dictionary<string, string>> assetTables = new Dictionary<string, Dictionary<string, string>>();

        /// <param name="assetLanguageTablePath">
        /// AssetDatabase path containing a {0} placeholder for the language code,
        /// for example: Packages/com.example.asset/Editor/Localization/Languages/{0}.txt
        /// Pass null when only the shared common strings are needed.
        /// </param>
        public EditorLocalization(string assetLanguageTablePath = null)
        {
            if (!string.IsNullOrWhiteSpace(assetLanguageTablePath) && !assetLanguageTablePath.Contains("{0}"))
                throw new ArgumentException("The asset language table path must contain a {0} language placeholder.", nameof(assetLanguageTablePath));

            this.assetLanguageTablePath = string.IsNullOrWhiteSpace(assetLanguageTablePath) ? null : assetLanguageTablePath;
        }

        public static string CurrentLanguage
        {
            get
            {
                string language = EditorPrefs.GetString(LanguagePreferenceKey, EnglishLanguageCode);
                return Array.IndexOf(LanguageCodes, language) >= 0 ? language : EnglishLanguageCode;
            }
            private set => EditorPrefs.SetString(LanguagePreferenceKey, value);
        }

        public bool DrawLanguageSelector()
        {
            int selectedIndex = Math.Max(0, Array.IndexOf(LanguageCodes, CurrentLanguage));
            EditorGUI.BeginChangeCheck();
            int newIndex = EditorGUILayout.Popup(Tr("common.language"), selectedIndex, LanguageNames);
            if (!EditorGUI.EndChangeCheck()) return false;

            CurrentLanguage = LanguageCodes[newIndex];
            return true;
        }

        public string Tr(string key)
        {
            string value;
            if (TryGetLocalizedValue(key, out value)) return value;

            if (ReportedMissingKeys.Add(key))
                Debug.LogWarning($"[Valen Commons] Missing editor translation: {key}");
            return key;
        }

        public string Format(string key, params object[] args)
        {
            return string.Format(Tr(key), args);
        }

        public GUIContent Content(string labelKey, string tooltipKey = null)
        {
            return string.IsNullOrEmpty(tooltipKey)
                ? new GUIContent(Tr(labelKey))
                : new GUIContent(Tr(labelKey), Tr(tooltipKey));
        }

        public GUIContent PropertyContent(SerializedProperty property, string labelKey)
        {
            string localizedTooltip;
            if (!TryGetLocalizedValue(labelKey + "_tooltip", out localizedTooltip))
                localizedTooltip = property.tooltip;

            return new GUIContent(Tr(labelKey), localizedTooltip);
        }

        private bool TryGetLocalizedValue(string key, out string value)
        {
            string language = CurrentLanguage;
            return TryGetAsset(language, key, out value) ||
                   TryGetCommon(language, key, out value) ||
                   TryGetAsset(EnglishLanguageCode, key, out value) ||
                   TryGetCommon(EnglishLanguageCode, key, out value);
        }

        private bool TryGetAsset(string language, string key, out string value)
        {
            if (assetLanguageTablePath == null)
            {
                value = null;
                return false;
            }

            return GetTable(assetTables, assetLanguageTablePath, language).TryGetValue(key, out value);
        }

        private static bool TryGetCommon(string language, string key, out string value)
        {
            return GetTable(CommonTables, CommonLanguageTablePath, language).TryGetValue(key, out value);
        }

        private static Dictionary<string, string> GetTable(
            Dictionary<string, Dictionary<string, string>> cache,
            string tablePath,
            string language)
        {
            Dictionary<string, string> table;
            if (cache.TryGetValue(language, out table)) return table;

            table = new Dictionary<string, string>();
            string path = string.Format(tablePath, language);
            TextAsset asset = AssetDatabase.LoadAssetAtPath<TextAsset>(path);
            if (asset == null)
            {
                Debug.LogError($"[Valen Commons] Could not load editor language table at {path}");
                cache[language] = table;
                return table;
            }

            string[] lines = asset.text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#", StringComparison.Ordinal)) continue;

                int separator = line.IndexOf('\t');
                if (separator <= 0) continue;

                string key = line.Substring(0, separator).Trim();
                string translatedValue = line.Substring(separator + 1).Replace("\\n", "\n");
                table[key] = translatedValue;
            }

            cache[language] = table;
            return table;
        }
    }
}
