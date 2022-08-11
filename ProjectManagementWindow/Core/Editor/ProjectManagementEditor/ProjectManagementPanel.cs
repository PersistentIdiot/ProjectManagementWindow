using System;
using JetBrains.Annotations;
using ProjectManagementWindow.Common;

namespace ProjectManagementWindow.Core.Editor.ProjectManagementEditor
{
    public class ProjectManagementPanel<T> where T : UnityEngine.Object
    {
        protected string _name;
        protected ScriptableObjectDatabase<T> _data;
        protected Type _type; // Unsure if needed. Thinking I might need reflection though, so keeping it

        public ProjectManagementPanel(string panelName, [CanBeNull] ScriptableObjectDatabase<T> panelData = default)
        {
            _name = panelName;
            _data = panelData;
            _type = typeof(T);
        }

        protected ProjectManagementPanel()
        {
            
        }
    }

    #region "Optional" Interfaces, ECSLite style
    // Bool return values indicate something failed, for (hopefully) easier testing.
    public interface IPanelDisplay<in T>
    {
        virtual protected bool Display(T data)
        {
            return true;
        }
    }
    
    // Display a vertical list of buttons to select elements of _data for display
    public interface IPanelDisplayMenu<in T>
    {
        virtual protected bool DisplayMenu(T data)
        {
            return true;
        }
    }

    // Display a horizontal list of buttons to select elements of _data for display
    public interface IPanelTab<in T>
    {
        virtual protected bool DisplayTab(T data)
        {
            return true;
        }
    }
    
    // Intended to be displayed in a container of some kind
    public interface IPanelDisplaySettings<in T>
    {
        
    }
    #endregion
}