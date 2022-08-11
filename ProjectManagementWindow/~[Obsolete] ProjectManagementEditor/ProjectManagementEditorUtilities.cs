using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor {
    public static class ProjectManagementEditorUtilities {
        // ToDo: Summary
        // Modified from TryGetUnityObjectsOfTypeFromPath<T>
        public static int TryGetUnityObjectsOfTypeFromPathRecursive<T>(string path, List<T> assetsFound) {
            string[] filePaths = System.IO.Directory.GetFiles(path);

            var directories = Directory.GetDirectories(path);
            foreach (var directory in directories) {
                TryGetUnityObjectsOfTypeFromPathRecursive(directory, assetsFound);
            }

            int countFound = 0;

            if (filePaths.Length <= 0) return countFound;
            foreach (string filePath in filePaths) {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
                if (obj is T asset) {
                    countFound++;
                    if (!assetsFound.Contains(asset)) {
                        assetsFound.Add(asset);
                    }
                }
            }

            return countFound;
        }

        /// <summary>
        /// Adds newly (if not already in the list) found assets.
        /// Returns how many found (not how many added)
        /// </summary>
        /// <typeparam name="T">Type of object to Try and Get</typeparam>
        /// <param name="path">Path used by AssetDatabase.LoadAssetAtPath</param>
        /// <param name="assetsFound">Adds to this list if it is not already there</param>
        /// <remarks>Found and modified from https://forum.unity.com/threads/loadallassetsatpath-not-working-or-im-using-it-wrong.110326/</remarks>
        /// <returns>int, so we can check count against zero.</returns>
        public static int TryGetUnityObjectsOfTypeFromPath<T>(string path, List<T> assetsFound) where T : UnityEngine.Object {
            string[] filePaths = Directory.GetFiles(path);

            int countFound = 0;

            if (filePaths.Length <= 0) return countFound;
            foreach (string filePath in filePaths) {
                UnityEngine.Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath(filePath, typeof(T));
                if (obj is T asset) {
                    countFound++;
                    if (!assetsFound.Contains(asset)) {
                        assetsFound.Add(asset);
                    }
                }
            }

            return countFound;
        }

        public static void BuildTree<T, TValue>(T value, Func<T, TValue[]> getChildren, TreeNode<TValue> head) {
            TValue[] children = getChildren.Invoke(value);
            foreach (var child in children) {
                TreeNode<TValue> branch = head.AddChild(child);
            }
        }

        public static void DrawDefaultSerializedObjectInspector<T>(SerializedObject _selectedObject) {
            
            if (_selectedObject.targetObject is T) {
                // Draw default inspector for object
                UnityEditor.Editor objectEditor = UnityEditor.Editor.CreateEditor(_selectedObject.targetObject);
                objectEditor.OnInspectorGUI();
            }
            else {
                string message = $"Unexpected type: {_selectedObject.targetObject?.GetType()} Expected {typeof(T)}!";
                Debug.Log($"{nameof(ProjectManagementEditorUtilities)}.{nameof(DrawDefaultSerializedObjectInspector)}() - {message}");
            }
        }
    }
}