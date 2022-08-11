namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs {
    /*
    /// <summary>
    /// Ability tab
    /// </summary>
    /// <typeparam name="TConfigData">string expected, as path</typeparam>
    /// <typeparam name="TData"></typeparam>
    public class AbilityTab<TConfigData> : ProjectManagementTabBase<TConfigData> {
        private const int SidebarWidth = 150;
        private const int SidebarSpacing = 5;
        private bool _sidebarVisible = true; // Unused for now
        
        private List<AbilityGroup> _data;
        
        private bool _initialized = false;
        private Vector2 _sidebarScrollPos;
        
        // Object selection
        protected SerializedObject _currentObject;
        protected SerializedObject _previousObject;
        
        public AbilityTab(TConfigData configData) {
            // @Gerold I'm doing this to make sure only the constructor can init. Would I be better off removing Init() in this case?
            // I feel like implementing it explicitly like this is best, because in this particular case I want to enforce this, but maybe not in all cases

            // Init kept as a separate method outside of constructor to allow user (AbilityTab)
            // to decide how to handle casting TConfigData=>String
            ((IProjectManagementTabData<TConfigData>)this).Init(configData);
        }

        // Init will be used to refresh, so instead of exposing Init, add ReInit and use that. Its job is to make sure data, etc is nulled out in case loading fails,
        // so we can passively test errors with loading. Not on interface, but may move it.
        // ToDo: @JonSelf find clean way to make this private, as mentioned in Constructor
        public override void Init(TConfigData configData) {

            if (configData is string stringData) {
                _data = BuildAbilityGroupList(stringData);
            }
            else {
                throw new Exception($"{nameof(AbilityTab<TConfigData>)}.Init() - Data was not expected type 'string', it was {configData.GetType()}");
            }

        }

        protected void EnsureInit(TConfigData configData) {
            if (_initialized) return;
            Init(configData);
        }

        public override void ReInit(TConfigData configData) {
            _initialized = false;
            EnsureInit(configData);
        }

        private List<AbilityGroup> BuildAbilityGroupList(string abilitiesPath) {
            var abilityGroupList = new List<AbilityGroup>();
            var abilities = new List<AbilityParametersBase>();

            TreeNode<string> directoryTree = new TreeNode<string>(abilitiesPath);
            ProjectManagementEditorUtilities.BuildTree(abilitiesPath, Directory.GetDirectories, directoryTree);
            directoryTree.Traverse(
                path => {
                    ProjectManagementEditorUtilities.TryGetUnityObjectsOfTypeFromPathRecursive(path, abilities);
                }
            );

            // Traverse tree, building AbilityGroupList
            directoryTree.Traverse(
                path => {
                    List<AbilityParametersBase> abs = new List<AbilityParametersBase>();
                    ProjectManagementEditorUtilities.TryGetUnityObjectsOfTypeFromPath(path, abs);

                    string folder = new DirectoryInfo(path).Name;
                    abilityGroupList.Add(new AbilityGroup(folder, abs));

                },
                node => { }
            );
            //Debug.Log(  $"{nameof(AbilityTab<TConfigData>)}.{nameof(BuildAbilityGroupList)}() - Message" + $"Added {abilityGroupList.Count} entries to AbilityTree");
            
            return abilityGroupList;
        }

        // ToDo: @JonSelf remove this parameter, cache it in Init.
        // AbilityTab's IProjectManagementTab~View() implementation
        public override void DisplayTab(ProjectManagementWindow.ToolbarEntry obj) {

            using (new EditorGUILayout.HorizontalScope()) {
                using (new EditorGUILayout.VerticalScope()) {

                    if (_sidebarVisible) DrawAbilitySidebar();
                }

                DrawSelectedAbility();
            }

        }

        private void DrawAbilitySidebar() {
            using (var sidebarScrollScope = new EditorGUILayout.ScrollViewScope(
                       _sidebarScrollPos,
                       GUILayout.MaxWidth(SidebarWidth),
                       GUILayout.ExpandHeight(false)
                   )) {
                _sidebarScrollPos = sidebarScrollScope.scrollPosition;

                foreach (AbilityGroup abilityGroup in _data) {
                    if (abilityGroup.Abilities.Count == 0) {
                        continue;
                    }
                    GUILayout.Space(SidebarSpacing);
                    GUILayout.Label(abilityGroup.GroupName);
                    foreach (var ability in abilityGroup.Abilities) {
                        if (GUILayout.Button(ability.DisplayName)) {
                            _currentObject = new SerializedObject(ability);
                        }
                    }
                }
            }
        }

        private void DrawSelectedAbility() {
            if (_currentObject == null) {
                EditorGUILayout.LabelField("Select an ability.");
                return;
            }

            // Cast to the expected type, and draw the object's default inspector.
            // ToDo: Convert this to a Utility method, with scope outside?
            if (_currentObject.targetObject is AbilityParametersBase ability) {
                using (new EditorGUILayout.VerticalScope(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true))) {
                    EditorGUILayout.ObjectField(ability, typeof(AbilityParametersBase), false);
                    UnityEditor.Editor objectEditor = UnityEditor.Editor.CreateEditor(ability);
                    using (new EditorGUILayout.VerticalScope()) {
                        objectEditor.OnInspectorGUI();
                    }
                }
            }
            else {
                Debug.Log($"Not Ability!");
            }
        }
    }

    
    [Serializable]
    public class AbilityGroup {
        public string GroupName;
        public List<AbilityParametersBase> Abilities;

        public AbilityGroup(string groupName = "Default Group Name", List<AbilityParametersBase> abilities = null) {
            GroupName = groupName;
            Abilities = abilities;
        }
    }
    */
}