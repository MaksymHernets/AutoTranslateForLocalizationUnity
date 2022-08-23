using GoodTime.HernetsMaksym.AutoTranslate.Editor;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GoodTime.HernetsMaksym.AutoTranslate.Windows
{
	public class SearchTextScene_EditorWindow : BaseSearchText_EditorWindow
    {
        // Window parameters
        private const string k_WindowTitle = "Search text for Localization in Scene";

        private Scene _currentScene;

        [MenuItem("Window/Auto Localization/Search Text for Tables in Scene")]
        public static void ShowWindow()
        {
            Type gameview = typeof(UnityEditor.EditorWindow).Assembly.GetType("UnityEditor.GameView");
            SearchTextScene_EditorWindow window = GetWindow<SearchTextScene_EditorWindow>(k_WindowTitle, true, typeof(SceneView), gameview);
            window.titleContent = new GUIContent(k_WindowTitle, EditorIcons.SearchTextScene);
            window.Show();
        }

        protected override void OnEnable()
        {
            UpdateLocalization();
            UpdateParameter();
        }

        protected override void OnFocus()
        {
            UpdateLocalization();
            UpdateParameter();
            _infoLocalization = string.Empty;
            _statusLocalizationScene = null;
        }

        private void UpdateParameter()
		{
            if ( _sharedStringTables!= null && _sharedStringTables.Count != 0) _selectedTable = _sharedStringTables.First().TableCollectionName;
            else _selectedTable = KEYWORD_NEWTABLE;

            _currentScene = SimpleDatabaseProject.GetCurrentScene();
            _nameTable = "StringTable_" + _currentScene.name + "_Scene";
        }

        private void OnGUI()
        {
            ShowNameWindow(k_WindowTitle);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Current Scene", GUILayout.Width(k_SeparationWidth));
            EditorGUILayout.LabelField(_currentScene.name);
            EditorGUILayout.EndHorizontal();

            Dropdown_StringTables(k_SeparationWidth);

            if ( _selectedTable == KEYWORD_NEWTABLE)
			{
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New string table", GUILayout.Width(k_SeparationWidth));
                _nameTable = EditorGUILayout.TextField("", _nameTable);
                EditorGUILayout.EndHorizontal();
            }
            
            Dropdown_SelectLanguage(k_SeparationWidth);

            EditorGUIUtility.labelWidth = k_SeparationWidth;
            _skipPrefab = EditorGUILayout.Toggle("Skip prefabs", _skipPrefab);

            if (GUILayout.Button("Search text for localization"))
            {
                _statusLocalizationScene = SearchTextForLocalization.CheckTextAboutLocalization(_currentScene, _skipPrefab);
            }

            if ( _statusLocalizationScene != null ) EditorGUILayout.HelpBox(_statusLocalizationScene.ToString() , MessageType.Info);

            CheckNameStringTable();

            ValidateLocalizationSettings();
            ValidateLocales();

            if (GUILayout.Button("Add localization"))
            {
                _infoLocalization = StartSearch();
            }

            if ( !string.IsNullOrEmpty(_infoLocalization) ) EditorGUILayout.HelpBox(_infoLocalization, MessageType.Info);
        }

        private string StartSearch()
		{
            SearchTextParameters parameters = new SearchTextParameters();

            if (_selectedTable == KEYWORD_NEWTABLE)
            {
                parameters.NameTable = _nameTable;
            }
            else
            {
                parameters.NameTable = _selectedTable;
            }

            if (string.IsNullOrEmpty(parameters.NameTable)) return "nameTable is null";

            parameters.IsSkipPrefab = _skipPrefab;
            parameters.SourceLocale = _selectedLocale;

            return SearchTextForLocalization.Search(parameters, _currentScene);
		}
    }
}