namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Obsolete
{
    // @Gerold You think this is worth keeping? Overrides inspector to only show paths and then a button for the rest
    // Temporarily disabled for testing and debugging
    /*
    [CustomEditor(typeof(ProjectManagemenSettings))]
    public class ProjectManagementSettingsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI() {
            // Only allow Paths to be edited, show button to edit other settings
            
            if (Selection.activeObject is ProjectManagemenSettings settingsObject) {
                settingsObject.AbilitiesPath = EditorGUILayout.TextField(nameof(ProjectManagemenSettings.AbilitiesPath), settingsObject.AbilitiesPath);
            }
            if (GUILayout.Button("Open Editor Window")) {
                ProjectManagementEditor.Open();
            }
        }
    }
    */
}
