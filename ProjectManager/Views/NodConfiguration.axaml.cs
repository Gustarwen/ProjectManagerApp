using System.ComponentModel.DataAnnotations;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class NodConfiguration : Window
{
    public NodConfiguration(Project selectedTask)
    {
        InitializeComponent();
        CanvasNodesViewModel informations = (CanvasNodesViewModel)DataContext;
        Accept.Click += (a, p) => {informations.OnClicked(new ProjectData(){ProjectId = informations.Project.Id, ParentId =selectedTask.Id, Name = NameBox.Text});};
    }
}