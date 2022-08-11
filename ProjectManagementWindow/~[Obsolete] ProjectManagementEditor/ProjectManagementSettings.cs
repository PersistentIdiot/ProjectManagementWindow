using System.Collections.Generic;
using System.Linq;
using ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;


// Leaving in the editor namespace, and putting in editor folder as I don't want any of this building
namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor {
    [CreateAssetMenu(
        menuName = "Project Management Window Settings",
        fileName = "New Project Management Window Settings", order = 0
    )]
    public class ProjectManagementSettings : ScriptableObject {
        // ToDo: Validate set, make sure these paths are valid. Done (here), but they're still serialized.
        // This means they can be edited in the prefab. Can't fix this until fixing editor window
        // bugging, not displaying at all, on invalid path. Otherwise, there's no way to fix a bad path
        // ToDo: Convert this to use UnityEditor.AssetDatabase.FindAssets($"t:{nameof(Abilities)}")?
        public string AbilitiesPath {
            get => _abilitiesPath;
            set {
                Debug.Assert(
                    AssetDatabase.IsValidFolder(value),
                    $"Attempted to set {nameof(AbilitiesPath)} to InvalidFolder, not setting!", this
                );
                if (AssetDatabase.IsValidFolder(value)) {
                    _abilitiesPath = value;
                }
                else {
                    Debug.Log($"oops");
                }
            }
        }
        public string PrefabsPath {
            get => _prefabsPath;
            set {
                Debug.Assert(
                    AssetDatabase.IsValidFolder(value),
                    $"Attempted to set {nameof(PrefabsPath)} to InvalidFolder, not setting!", this
                );
                if (AssetDatabase.IsValidFolder(value)) {
                    _prefabsPath = value;
                }
            }
        }

        public List<FolderGroup> FolderGroups { get => folderGroups; }
        [SerializeField] private List<FolderGroup> folderGroups;

        [FormerlySerializedAs("AbilitiesPath")] [SerializeField]
        private string _abilitiesPath;
        [FormerlySerializedAs("PrefabsPath")] [SerializeField]
        private string _prefabsPath;

        private static string
            _dataPath; // Is this needed? Only here because MultiplayerSettings was using it. Seems to be caching for later use, but not sure why.
        private static ProjectManagementSettings _instance;
        public static ProjectManagementSettings Instance {
            get {
            #if UNITY_EDITOR
                if (!_instance) {
                    string assetId = UnityEditor.AssetDatabase.FindAssets($"t:{nameof(ProjectManagementSettings)}")
                        .FirstOrDefault();
                    if (!string.IsNullOrEmpty(assetId)) {
                        Debug.Log(
                            $"Using {nameof(ProjectManagementSettings)} at '{UnityEditor.AssetDatabase.GUIDToAssetPath(assetId)}' ({assetId}) as singleton reference."
                        );
                        _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<ProjectManagementSettings>(
                            UnityEditor.AssetDatabase.GUIDToAssetPath(assetId)
                        );
                        _dataPath = Application.dataPath;
                    }
                }

                if (_instance == null)
                    Debug.LogError(
                        $"No instance of {nameof(ProjectManagementSettings)} (t:{nameof(ProjectManagementSettings)}) was found."
                    );
            #endif
                return _instance;
            }
        }

        // Can be set to false after Init by RefreshDatabase, in order to make sure groups are nulled out, to test RefreshDatabase passively.
        private bool _initialized = false;

        // Only exists in case we need to null something out for testing via RefreshDatabase
        public void EnsureInit() {
            if (_initialized) return;
            Init();
        }

        private void Init() {
            // Nothing to actually initialize anymore. Leaving in case of future needs. Maybe check if paths are valid here?
            _initialized = true;
        }

        public void RefreshDatabase() {
            _initialized = false;
            EnsureInit();
        }
    }
}