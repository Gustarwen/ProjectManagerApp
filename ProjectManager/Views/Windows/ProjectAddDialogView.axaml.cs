using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.Views.Windows;

namespace ProjectManager.Views;

public partial class ProjectAddDialogView : NodConfiguration
{
    public ProjectAddDialogView(ProjectData selectedTask,CanvasNodesView nodesview) : base(selectedTask,nodesview)
    {

        InitializeComponent();
        
        
    }
}