using CommunityToolkit.Mvvm.ComponentModel;
using ProjectManager.DataTemplates;


namespace ProjectManager.ViewModels;

public partial class ProjectBoardViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _selected;
    [ObservableProperty] private ViewModelBase? _graph;
    [ObservableProperty] private ViewModelBase? _piegraph;
    [ObservableProperty] private ViewModelBase? _blockgraph;
    public ProjectBoardViewModel(Project project)
    {
        Selected = new CanvasNodesViewModel(project);
        Graph = new graphViewModel();
        Piegraph =  new piegraphViewModel();
        Blockgraph = new blockGraphViewModel();

    }
}