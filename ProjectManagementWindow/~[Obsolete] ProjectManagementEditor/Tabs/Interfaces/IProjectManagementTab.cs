namespace ProjectManagementWindow.Core.Editor._Obsolete__ProjectManagementEditor.Tabs.Interfaces {
    // ToDo: Instructions for how to create a new tab for @Jon and @Gerold on Github Discussions
    // ToDo: Implement MultiplayerSettings tab with this, see how long it takes, to use as a measure of usefulness of this

    /// <summary>
    /// Combined interface for TabData/View with a label for the tab.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TConfigData"></typeparam>
    /// <remarks>
    /// <para>The intended use case and reason these are all in one file.</para>
    /// </remarks>
    public interface IProjectManagementTab<TConfigData> : IProjectManagementTabData<TConfigData>, IProjectManagementTabView {
    }

    /// <summary>
    /// Interface to hold and initialize data.
    /// Interface which ProjectManagementSettings will use to initialize data needed for this tab.
    /// Creates a contract between user class and ProjectManagementSettings.
    /// ProjectManagementSettings supplies the config data, user sets it up and inits it and holds it.
    /// ProjectManagementSettings uses IProjectmanagementTabView to display it.
    /// </summary>
    /// <remarks></remarks>
    /// <typeparam name="ProjectManagementSettings">ScriptableObject singleton initializing and holding data.</typeparam>
    /// <typeparam name="TData">Type of data the tab will need initialized.</typeparam>
    public interface IProjectManagementTabData<TConfigData> {
        //TData _data { get; } // We don't care if the user holds the data, only that they initialize it.
        void Init(TConfigData configData);
        void ReInit(TConfigData configData);
    }

    /// <summary>
    /// Interface that allows a user of this class to display an editor tab in its entirety.
    /// <remarks>In our use case, ProjectManagementSettings will call this, so Vertical/Horizontal scopes may affect it.</remarks>
    /// </summary>
    public interface IProjectManagementTabView {
        void DisplayTab(ProjectManagementWindow.ToolbarEntry obj);
    }
}