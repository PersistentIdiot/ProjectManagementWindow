using System;
using System.Collections.Generic;
using System.IO;
using ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs.Interfaces;
using UnityEditor;
using UnityEngine;


namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs
{
    // ToDo: Move to new file after verifying. Verify this before changing PrefabsTab if possible
    // ToDo: Remove generics from PrefabsTab, use concrete types

    // ToDo: Move this inside PrefabsTab when finished

    public class PrefabsTab<TConfigData> : ProjectManagementTabBase<TConfigData>
    {
        private const int SidebarWidth = 150;
        private const int SidebarSpacing = 5;
        private bool _sidebarVisible = true; // Unused for now
        
        private List<PrefabGroup> _data;

        private bool _initialized = false;
        private Vector2 _sidebarScrollPos;
        private Vector2 _prefabScrollPos;
        

        protected const int PrefabHeaderHeight = 50;

        // Object selection
        private SerializedObject _currentObject;
        private SerializedObject _previousObject;
        private Action OnObjectSelected;
        PrefabEditorData _cachedPrefabData;

        public PrefabsTab(TConfigData configData) : base(configData)
        {
            // @Gerold I'm doing this to make sure only the constructor can init. Would I be better off removing Init() in this case?
            // I feel like implementing it explicitly like this is best, because in this particular case I want to enforce this, but may not want to in all (general) cases

            // Init kept as a separate method outside of constructor to allow user (PrefabsTab)
            // to decide how to handle casting TConfigData=>String
            ((IProjectManagementTabData<TConfigData>)this).Init(configData);
        }

        public override void DisplayTab(ProjectManagementWindow.ToolbarEntry obj)
        {

            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUILayout.VerticalScope())
                {

                    if (_sidebarVisible) DrawPrefabsSidebar();
                }
                if (_currentObject != _previousObject)
                {
                    Debug.Log($"{nameof(PrefabsTab<TConfigData>)}.{nameof(_currentObject)}() - Object changed!");
                    OnObjectSelected?.Invoke();
                    _previousObject = _currentObject;
                }
                DrawSelectedPrefab();
            }
        }

        public override void Init(TConfigData configData)
        {
            OnObjectSelected = UpdateSelectedObject;
            if (configData is string stringData)
            {
                _data = BuildPrefabsGroupList(stringData);
            }
            else
            {
                throw new Exception($"{nameof(PrefabsTab<TConfigData>)}.Init() - Data was not expected type 'string', it was {configData.GetType()}");
            }
        }

        // Build and cache structure containing info needed to display folding view of prefab
        private void UpdateSelectedObject() { _cachedPrefabData = new PrefabEditorData(_currentObject); }

        protected void EnsureInit(TConfigData configData)
        {
            if (_initialized) return;
            Init(configData);
        }

        public override void ReInit(TConfigData configData)
        {
            _initialized = false;
            EnsureInit(configData);
        }

        private List<PrefabGroup> BuildPrefabsGroupList(string prefabsPath)
        {
            var prefabsGroupList = new List<PrefabGroup>();
            var prefabs = new List<GameObject>();

            TreeNode<string> directoryTree = new TreeNode<string>(prefabsPath);
            ProjectManagementEditorUtilities.BuildTree(prefabsPath, Directory.GetDirectories, directoryTree);
            directoryTree.Traverse(
                path => {
                    ProjectManagementEditorUtilities.TryGetUnityObjectsOfTypeFromPathRecursive(path, prefabs);
                }
            );

            // Traverse tree, building PrefabsGroupList
            directoryTree.Traverse(
                path => {
                    List<GameObject> prefabs = new List<GameObject>();
                    ProjectManagementEditorUtilities.TryGetUnityObjectsOfTypeFromPath(path, prefabs);

                    string folder = new DirectoryInfo(path).Name;
                    prefabsGroupList.Add(new PrefabGroup(folder, prefabs));

                }, node => { }
            );

            // ToDo: @JonSelf consider caching reference to Project Window, and adding a "Debugging" toggle for things like this.
            //Debug.Log( $"{nameof(PrefabsTab<TConfigData>)}.{nameof(BuildPrefabsGroupList)}() - " + $"Added {prefabsGroupList.Count} entries to PrefabsTree");
            return prefabsGroupList;
        }

        private void DrawPrefabsSidebar()
        {
            using (var sidebarScrollScope = new EditorGUILayout.ScrollViewScope(
                       _sidebarScrollPos, GUILayout.MaxWidth(SidebarWidth), GUILayout.ExpandHeight(true)
                   ))
            {
                _sidebarScrollPos = sidebarScrollScope.scrollPosition;

                foreach (PrefabGroup prefabGroup in _data)
                {
                    if (prefabGroup.Prefabs.Count == 0)
                    {
                        continue;
                    }
                    GUILayout.Space(SidebarSpacing);
                    GUILayout.Label(prefabGroup.GroupName);
                    foreach (var prefab in prefabGroup.Prefabs)
                    {
                        // ToDo: Consider trying to do better than just the GO name. Need a good way to add metadata to the prefab for editor stuff like this maybe
                        if (GUILayout.Button(prefab.name))
                        {
                            _currentObject = new SerializedObject(prefab);
                        }
                    }
                }
            }
        }

        private void DrawSelectedPrefabHeader()
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.MaxHeight(PrefabHeaderHeight), GUILayout.ExpandHeight(false)))
            {
                if (_cachedPrefabData == null)
                {
                    EditorGUILayout.LabelField("Select a Prefab.");
                }
                else
                {
                    EditorGUILayout.ObjectField(_cachedPrefabData.TargetSerializedObject.targetObject, typeof(GameObject), false);
                }
            }
        }

        private void DrawSelectedPrefab()
        {
            //if (_cachedPrefabData == null) return;
            using (new EditorGUILayout.VerticalScope())
            {
                DrawSelectedPrefabHeader();
                //_cachedPrefabData.DisplaySlow(ref _prefabScrollPos, _currentObject);
                _cachedPrefabData?.Display(ref _prefabScrollPos);
            }

        }
    }

    internal class PrefabEditorData
    {
        public SerializedObject TargetSerializedObject { get => _targetSerializedObject; }
        private SerializedObject _targetSerializedObject;

        private string _name;
        private List<UnityEditor.Editor> _editors;
        private List<bool> _foldouts; // True => Expanded, False = > collapsed

        public PrefabEditorData(SerializedObject targetSerializedObject)
        {
            // Set targetObject and reinit data no default
            _targetSerializedObject = targetSerializedObject;
            _editors = new List<UnityEditor.Editor>();
            _foldouts = new List<bool>();

            BuildCache(_targetSerializedObject, ref _name);

        }

        
        // ToDo make generic utility method out of this? T = GameObject in this case.
        // Figure out another use for it first, this is the only tab complex enough to need one so far
        // SerializedObject targetSerializedObject => GameObject => List<Component> => List<Editor> _editors
        void BuildCache(SerializedObject inputSerializedObject, ref string inputName)
        {
            // Return early, leaving things at default values if null
            if (_targetSerializedObject == null) return;
            Debug.Log($"Rebuilding selected prefab editor cache");

            if (!(inputSerializedObject.targetObject is GameObject selectedPrefabGameObject))
            {

                Debug.Log(
                    $"{nameof(PrefabEditorData)}.{nameof(BuildCache)}() - Expected GameObject, got {inputSerializedObject.targetObject.GetType()}"
                );
                return;
            }

            inputName = selectedPrefabGameObject.name;
            
            Component[] components = selectedPrefabGameObject.GetComponents<Component>();
            foreach (Component component in components)
            {
                UnityEditor.Editor objectEditor = UnityEditor.Editor.CreateEditor(component);
                _editors.Add(objectEditor);
                _foldouts.Add(false);
            }
        }

        public void Display(ref Vector2 _prefabScrollPos)
        {
            if (TargetSerializedObject == null || _editors == null || _editors.Count == 0)
            {
                EditorGUILayout.LabelField("Select a Prefab.");
                return;
            }

            using (var prefabScrollScope = new EditorGUILayout.ScrollViewScope(_prefabScrollPos))
            {
                _prefabScrollPos = prefabScrollScope.scrollPosition;
                for (int i = 0; i < _editors.Count; i++)
                {
                    //_editors[i].DrawDefaultInspector();

                    _foldouts[i] = EditorGUILayout.Foldout(_foldouts[i], _editors[i].target.GetType().Name.ToString());
                    if (_foldouts[i])
                    {

                        EditorGUIUtility.labelWidth = 0;
                        _editors[i].DrawHeader();

                        EditorGUI.BeginChangeCheck();
                        _editors[i].OnInspectorGUI();

                        //When the object is changed it is reimported, and our editors point to incorrect objects. Restart to create new editors!
                        if (EditorGUI.EndChangeCheck())
                        {
                            //OnEnable();
                            return;
                        }
                    }

                }
            }
        }
    }

    // ToDo: genericize this in ProjectManagementTabBase? Or not
    // Don't genericize this, its behavior is specific to this tab
    [Serializable]
    internal class PrefabGroup
    {
        public string GroupName;
        public List<GameObject> Prefabs;

        public PrefabGroup(string groupName = "Default Group Name", List<GameObject> prefabs = null)
        {
            GroupName = groupName;
            Prefabs = prefabs;
        }
    }
}