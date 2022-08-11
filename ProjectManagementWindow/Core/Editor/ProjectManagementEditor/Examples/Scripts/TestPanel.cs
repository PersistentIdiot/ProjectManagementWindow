using ProjectManagementWindow.Core.Editor.ProjectManagementEditor.Examples;

namespace ProjectManagementWindow.Core.Editor.ProjectManagementEditor
{
    public class TestPanel: ProjectManagementPanel<TestScriptableObject>
    {
        private ProjectManagementPanel<TestScriptableObject> _data;

        public TestPanel(ProjectManagementPanel<TestScriptableObject> panelData)
        {
            _data = panelData;
        }

        public bool TryInit()
        {
            return Init();
        }

        private bool Init()
        {
            return true;
        }

        public void Update()
        {

        }
    }
}