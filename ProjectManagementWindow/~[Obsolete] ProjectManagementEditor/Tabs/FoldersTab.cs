using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs.Interfaces;
using UnityEditor;
using UnityEngine;

namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs
{
    public class FoldersTab<TConfigData> : ProjectManagementTabBase<TConfigData>
    {
        private List<FolderGroup> _data;

        private const int SidebarWidth = 250;
        private const int SidebarSpacing = 5;
        private bool _sidebarVisible = true; // Unused for now

        private Vector2 _sidebarScrollPos;
        private string newPath;
        private string newPathName;
        TConfigData @null;
        public FoldersTab() { ((IProjectManagementTabData<TConfigData>)this).Init(@null); }

        public override void DisplayTab(ProjectManagementWindow.ToolbarEntry obj)
        {
            using (new GUILayout.HorizontalScope())
            {
                DrawSideBar();
                DisplayMainArea();
            }

        }

        private void DrawSideBar()
        {
            using (var sidebarScrollScope = new EditorGUILayout.ScrollViewScope(_sidebarScrollPos, GUILayout.MaxWidth(SidebarWidth)))
            {
                _sidebarScrollPos = sidebarScrollScope.scrollPosition;

                // Iterating backwards because of remove button
                for (int i = ProjectManagementSettings.Instance.FolderGroups.Count - 1; i >= 0; i--)
                {
                    using (new GUILayout.HorizontalScope())
                    {

                        if (GUILayout.Button("X", GUILayout.MaxWidth(50)))
                        {
                            ProjectManagementSettings.Instance.FolderGroups.RemoveAt(i);
                            break;
                        }

                        if (GUILayout.Button(ProjectManagementSettings.Instance.FolderGroups[i].PathName))
                        {

                            // Load object
                            UnityEngine.Object target = AssetDatabase.LoadAssetAtPath(
                                ProjectManagementSettings.Instance.FolderGroups[i].Path, typeof(UnityEngine.Object)
                            );

                            // Select the object in the project folder
                            Selection.activeObject = target;

                            // Also flash the folder yellow to highlight it
                            EditorGUIUtility.PingObject(target);
                        }
                    }
                }
            }
        }

        private void DisplayMainArea()
        {

            var EntryValidator = EntryValidation();
            if (GUILayout.Button(new GUIContent($"Add Path {EntryValidator.OutputString}", EntryValidator.Errors)) && EntryValidator.IsValid)
            {
                FolderGroup folderGroup = new FolderGroup(newPathName, newPath);
                if (!ProjectManagementSettings.Instance.FolderGroups.Contains(folderGroup))
                {
                    ProjectManagementSettings.Instance.FolderGroups.Add(folderGroup);
                    ReInit(@null);
                }
                else
                {
                    Debug.LogError("Path already exists!");
                }

            }
            newPathName = EditorGUILayout.TextField("Path Name", newPathName);
            newPath = EditorGUILayout.TextField("Path", newPath);

        }

        // Veto pattern with output string, error string output and bool value
        private (string OutputString, string Errors, bool IsValid) EntryValidation()
        {
            List<string> errors = new List<string>();
            string errorString = "";
            bool pathIsValid = true;
            bool entryIsValid = true;
            bool nameIsValid = true;

            // Make sure path isn't empty, and that it exists
            pathIsValid &= !string.IsNullOrEmpty(newPath);
            if (!pathIsValid) errors.Add("Path is null");
            pathIsValid &= Directory.Exists(newPath);
            if (!pathIsValid) errors.Add("Directory doesn't exist");

            // Make sure PathName isn't empty
            nameIsValid &= !string.IsNullOrEmpty(newPathName);
            if (!nameIsValid) errors.Add("Path name empty");

            // Make sure the directory or name isn't already in list, or name/path are invalid
            entryIsValid &= pathIsValid && nameIsValid;
            
            // Further branching validation
            if (entryIsValid)
            {
                entryIsValid &= ProjectManagementSettings.Instance.FolderGroups.All(group => group.Path != newPath);
                if (!entryIsValid) errors.Add($"Path '{newPath}' already exists");
                entryIsValid &= ProjectManagementSettings.Instance.FolderGroups.All(group => group.PathName != newPathName);
                if (!entryIsValid) errors.Add($"Path Name '{newPathName}' already exists");
            }

            // Convert errors to string
            for (int i = 0; i < errors.Count; i++)
            {
                errorString += errors[i] + (i + 1 > errors.Count ? ", \n" : ". \n");
            }

            return (entryIsValid ? char.ConvertFromUtf32(0x2611) : char.ConvertFromUtf32(0x2610), errorString, pathIsValid);
        }

        public override void Init(TConfigData configData) { }

        public override void ReInit(TConfigData configData) { Init(configData); }
    }

    // @JonSelf - Should maybe be using tuples like EntryValidation above for other Groups instead of classes
    // (List<GameObject> things, string path) BuildThingsFromPath() { return (new List<GameObject>(), "path"); }
    // Each tab can define a static method to build them?
    [Serializable]
    public class FolderGroup
    {
        public string PathName;
        public string Path;

        public FolderGroup(string pathName, string path)
        {
            PathName = pathName;
            Path = path;
        }
    }
}