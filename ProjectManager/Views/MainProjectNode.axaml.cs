using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class MainProjectNode : UserControl
{
    ProjectData projectData;
    public MainProjectNode(ProjectData project)
    {
       
        InitializeComponent();
        taskViewModel viewModel = new taskViewModel();
        projectData = project;
        Name.Text = project.Name;
        rollDown.Command = viewModel.unRollCommand;

    }
}