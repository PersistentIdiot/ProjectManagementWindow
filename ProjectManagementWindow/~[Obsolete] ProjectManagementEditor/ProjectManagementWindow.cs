using System;
using System.Collections.Generic;
using ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs.Interfaces;
using UnityEditor;
using UnityEngine;

namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor {
    /// <summary>
    /// Editor window for various project management tasks.
    /// Currently supports a list of Abilities, and a lit of Prefabs.
    /// These are generated in ProjectManagementSettings via AssetDatabase.LoadAssetAtPath,
    /// after creating a tree of subdirectories and then traversing it to build these lists.
    /// ProjectManagementSettings maintains and exposes the lists generated from these trees,
    /// but not the trees themselves (yet).
    /// <remarks>ToDo: Enforce the separation of tabs, separate these methods out and update ProjectManagementSettings to reflect this division of concerns. </remarks>
    /// </summary>
    // ToDo: @POSTREMOVAL Rename after removing old version
    public class ProjectManagementWindow : EditorWindow {
        public struct ToolbarEntry {
            [Obsolete]
            public string Label;

            private Action<ToolbarEntry> _drawTab;

            public ToolbarEntry(string label, Action<ToolbarEntry> drawTab)
            {
                Label = label;
                _drawTab = drawTab;
            }

            // ToDo: Is this needed?
            // @Gerold thoughts? I feel like it is because in general we wouldn't want someone able to access
            // _drawTab and clear it or something but in our case it doesn't matter. I'm just hoping to
            // extract this (UpdatedProjectManagementEditor idea) later to use in other projects, also I'm curious
            public void DrawTab()
            {
                _drawTab?.Invoke(this);
            }
        }

        // Settings
        private const string WindowTitle = "Project Management Window";
        private const int HeaderMinHeight = 10;

        #region State Related
        private bool _sidebarVisible = true; // Unused (for now)
        private bool _advancedSettingsVisible = false;
        private int _toolbarSelectionIndex = 0;
        private bool _initialized = false;

        // Misc
        private static ProjectManagementWindow _window;
        private ProjectManagementSettings settings;
        private SerializedObject _serializedObject; // Do we want ClearData to clear this?

        private List<ToolbarEntry> ToolbarEntries;

        [Obsolete]
        private string[] _toolbarButtonLabels;

        // Tabs ToDo: @JonSelf Group up, maybe combine with ToolbarEntry
        private IProjectManagementTabView abilityTab;
        private IProjectManagementTabView prefabsTab;
        private IProjectManagementTabView foldersTab;
        private IProjectManagementTabView multiplayerSettingsTab;
        #endregion

        [MenuItem("Project/Project Management")]
        static void UpdatedProjectManagementWindow()
        {
            var editorAsm = typeof(UnityEditor.Editor).Assembly;
            var inspWndType = editorAsm.GetType("UnityEditor.SceneHierarchyWindow") ??
                              editorAsm.GetType("UnityEditor.InspectorWindow");
            _window = GetWindow<ProjectManagementWindow>(inspWndType);
            _window.titleContent = new GUIContent(WindowTitle);
        }

        public void EnsureInit()
        {
            if (_initialized)
            {
                // Debug.Log($"{nameof(UpdatedProjectManagementEditor)}.{nameof(EnsureInit)}() - Already initialized!");
                return;
            }
            // Bug: @JonSelf  When paths are incorrect, exceptions cause the entire window to fail rendering. 
            // Easy fix but gross = cache last working paths in RefreshDataBase, wrap EnsureInit call in try/catch and revert on fail
            Init();
            _initialized = true;
        }

        private void Init()
        {
            if (_window == null) _window = this;
            if (_serializedObject == null) return;
            // Make sure settings is expected type, and cache it. Else throw a long exception
            if (_serializedObject.targetObject is ProjectManagementSettings projectManagementSettings)
            {
                settings = projectManagementSettings;
                settings.RefreshDatabase();
            }
            else
            {
                // Long exception is long. Convert to const to shorten?
                string expectedType = $"{nameof(ProjectManagementSettings)}";
                string actualType = $"{(settings == null ? "Null" : settings.GetType().ToString())}";
                string message = $"{nameof(settings)} was not type: {expectedType}, actual Type: {actualType}";
                throw new Exception($"{nameof(ProjectManagementWindow)}.{nameof(Init)}() - {message}");
            }

            _sidebarVisible = true;

            ProjectManagementSettings.Instance.EnsureInit();

            InitTabs();
            _initialized = true;
            //Debug.Log( $"{nameof(ProjectManagementWindow)}.{nameof(Init)}() -" + $" Initialized Successfully! ToDo: Make sure this isn't initializing for unrelated recompiles, etc");
        }

        private void InitTabs()
        {
            /*
            // Make sure we init once, but allow resetting of IsInitialized
            // ToDo: Use EnsureInit
            string abilitiesPath = ProjectManagementSettings.Instance.AbilitiesPath;
            string prefabsPath = ProjectManagementSettings.Instance.PrefabsPath;

            abilityTab = new AbilityTab<string>(abilitiesPath);
            prefabsTab = new PrefabsTab<string>(prefabsPath);
            multiplayerSettingsTab = new MultiplayerSettingsTab<string>();
            foldersTab = new FoldersTab<string>();

            ToolbarEntries = new List<ToolbarEntry>
            {
                new ToolbarEntry("Abilities", abilityTab.DisplayTab),
                new ToolbarEntry("Prefabs (WIP)", prefabsTab.DisplayTab),
                new ToolbarEntry("Folders", foldersTab.DisplayTab),
                new ToolbarEntry("Multiplayer Settings", multiplayerSettingsTab.DisplayTab)
                // ,new ToolbarEntry("Folders (Not implemented, left for testing. Null DrawTab Method and long name.)", null)
            };

            // Cache the labels, so we're not running Linq in OnGUI. ToDo: just run in DrawHeader, and use the Tabs themselves. @REMOVE this
            _toolbarButtonLabels = ToolbarEntries.Select(x => x.Label).ToArray();
            if (_toolbarButtonLabels == null)
            {
                Debug.Log($"{nameof(ProjectManagementWindow)}.{nameof(InitTabs)}() Labels null!");
            }else if (_toolbarButtonLabels.Length ==0)
            {
                Debug.Log($"{nameof(ProjectManagementWindow)}.{nameof(InitTabs)}() Labels length 0!");
            }
            */
        }

        private void OnEnable()
        {
            _initialized = false;
            EnsureInit();
        }

        private void OnFocus()
        {
            if (_serializedObject == null)
            {
                _serializedObject = new SerializedObject(ProjectManagementSettings.Instance);
            }

            EnsureInit();
        }

        private void OnGUI()
        {
            // Remove after figuring out why it's required here
            EnsureInit();

            // Main scope
            using (new GUILayout.VerticalScope())
            {
                using (new GUILayout.VerticalScope(GUILayout.MinHeight(HeaderMinHeight)))
                {
                    DrawHeader();
                }

                using (new EditorGUILayout.HorizontalScope())
                {
                    // ToDo: If these are just being registered in Init, there's no need for them to be in this file. Separate file per tab might be better
                    if (ToolbarEntries == null)
                    {
                        Debug.Log($"Entries null!");
                    }
                    else
                    {
                        ToolbarEntries[_toolbarSelectionIndex].DrawTab();
                    }
                }
            }
        }

        private void RefreshButtonClick()
        {
            Debug.Log($"{nameof(ProjectManagementWindow)}.{nameof(RefreshButtonClick)}()");

            // Ensures EnsureInit gets a "full cycle" for passive testing
            try
            {

                ClearInitData();
                EnsureInit();
            }
            catch (Exception e)
            {
                throw new Exception($"{nameof(ProjectManagementWindow)}.{nameof(RefreshButtonClick)}()\n " +
                                    $"An exception occured when Refreshing. \n " +
                                    $"ToDo: Try doesn't fix anything, undo? \n " +
                                    $"Exception: {e}");
            }

        }

        private void ClearInitData()
        {
            // ToDo: Group variables cleared by this so it's more explicit, or just move this near them?
            _initialized = false;
            _window = null;
            settings = null;
            ToolbarEntries = null;
            _toolbarButtonLabels = null;
            abilityTab = null;
            prefabsTab = null;
            multiplayerSettingsTab = null;
        }

        private void DrawHeader()
        {
            _advancedSettingsVisible = EditorGUILayout.Foldout(_advancedSettingsVisible, "Advanced Settings");
            if (_advancedSettingsVisible)
            {
                // Object field only for display, to select the object from window. No assignment here.
                EditorGUILayout.ObjectField(_serializedObject.targetObject, typeof(ProjectManagementSettings), false);

                // AbilitiesPath
                //Undo.RecordObject(settings, "AbilitiesPath");
                settings.AbilitiesPath = EditorGUILayout.TextField(settings.AbilitiesPath);

                // Prefabs path
                //Undo.RecordObject(settings, "PrefabsPath");
                settings.PrefabsPath = EditorGUILayout.TextField(settings.PrefabsPath);

                // ToDo: Modal dialog to confirm
                if (GUILayout.Button("Refresh Database"))
                {
                    RefreshButtonClick();
                }

                _sidebarVisible = EditorGUILayout.ToggleLeft("Show Sidebar (Broken)", _sidebarVisible);

            }

            if (_toolbarButtonLabels != null )
            {
                
                _toolbarSelectionIndex = GUILayout.Toolbar(_toolbarSelectionIndex, _toolbarButtonLabels);
            }
            else
            {
                InitTabs();
            }

        }
    }
}