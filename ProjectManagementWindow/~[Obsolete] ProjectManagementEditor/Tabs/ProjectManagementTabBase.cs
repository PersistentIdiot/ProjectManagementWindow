using ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs.Interfaces;

namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs {
    public abstract class ProjectManagementTabBase<TConfigData> : IProjectManagementTab<TConfigData>  {
        // State data
        protected TConfigData Label { get; }

        // ToDo: @JonSelf Move these editor specific fields into IProjectManagementTab,
        // or break them up into smaller interfaces to make tabs configurable, to maybe genericize this more.
        // State data and settings
        
        
        
        protected ProjectManagementTabBase() {
        }

        protected ProjectManagementTabBase(TConfigData configData) { Label = configData; }

        // ToDo: Double check methods below for possible generics and rename abstractly
        //protected abstract List<PrefabGroup> BuildPrefabsGroupList(string prefabsPath);

        // AbilityTab's IProjectManagementTab~View() implementation
        public abstract void DisplayTab(ProjectManagementWindow.ToolbarEntry obj);

        public abstract void Init(TConfigData configData);
        public abstract void ReInit(TConfigData configData);
    }
}