namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs {
    /*
    public class MultiplayerSettingsTab<TConfigData> : ProjectManagementTabBase<TConfigData> {
        private MultiplayerSettings _data;

        public override void DisplayTab(ProjectManagementWindow.ToolbarEntry obj) {
            //ProjectManagementEditorUtilities.DrawDefaultSerializedObjectInspector<MultiplayerSettings>(new SerializedObject(_data));
            if (_data == null) {
                Debug.Log($"{nameof(MultiplayerSettingsTab<TConfigData>)}.{nameof(DisplayTab)}() - Data was null!");
                _data = MultiplayerSettings.Instance;
            }

            // ToDo: @JonSelf, figure out how to draw the custom inspector for this.
            using (new GUILayout.VerticalScope()) {
                EditorGUILayout.ObjectField(_data, typeof(MultiplayerSettings), false);
                ProjectManagementEditorUtilities.DrawDefaultSerializedObjectInspector<MultiplayerSettings>(new SerializedObject(_data));
            }
        }

        public override void Init(TConfigData configData) { _data = MultiplayerSettings.Instance; }

        public override void ReInit(TConfigData configData) { Init(configData); }
    }
    */
}