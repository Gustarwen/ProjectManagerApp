using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ProjectManager.DataTemplates;

namespace ProjectManager.ViewModels;

public partial class ProjectBoardViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? selected;
    public ProjectBoardViewModel(Project project)
    {
        Selected = new canvasNodesViewModel(project);
    }
}