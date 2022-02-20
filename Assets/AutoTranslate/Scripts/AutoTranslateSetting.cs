using System;
using UnityEditor;
using UnityEngine;

namespace GoodTime.Tools.InterfaceTranslate
{
    public class AutoTranslateSetting : ScriptableObject
    {
        public const string k_MyCustomSettingsPath = "Assets/AutoTranslate/ProjectSettings/AutoTranslateSetting.asset";

        [SerializeField] public TypePlatformTranslate PlatformForTranslate;

        [NonSerialized] public string[] Platforms;

        public static AutoTranslateSetting GetOrCreateSettings()
        {
            AutoTranslateSetting settings = AssetDatabase.LoadAssetAtPath<AutoTranslateSetting>(k_MyCustomSettingsPath);
            string[] platforms = Enum.GetNames(typeof(TypePlatformTranslate));
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<AutoTranslateSetting>();
                settings.Platforms = platforms;
                if (platforms.Length != 0)
                {
                    settings.PlatformForTranslate = TypePlatformTranslate.GoogleApisCustom;
                }
                AssetDatabase.CreateAsset(settings, k_MyCustomSettingsPath);
                AssetDatabase.SaveAssets();
            }
            settings.Platforms = platforms;
            return settings;
        }

        internal static SerializedObject GetSerializedSettings()
        {
            return new SerializedObject(GetOrCreateSettings());
        }
    }
}
