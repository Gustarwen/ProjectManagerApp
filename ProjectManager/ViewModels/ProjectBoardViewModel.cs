using System;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ProjectManager.DataTemplates;
using ProjectManager.Views;


namespace ProjectManager.ViewModels;

public partial class ProjectBoardViewModel : ViewModelBase
{
    [ObservableProperty] private ViewModelBase? _selected;
    [ObservableProperty] private ViewModelBase? _graph;
    [ObservableProperty] private ViewModelBase? _piegraph;
    [ObservableProperty] private ViewModelBase? _blockgraph;
    public Project project;
    public  void RefreshProgress()
    {
        var tasks = DataBaseController.Taskrepository.Query()
            .Where(p => p.ProjectId == project.Id).Execute().ToList();
        double ratio = tasks.Count == 0 ? 0 : (double)tasks.Count(t => t.Finished) / tasks.Count;
        if (Piegraph is piegraphViewModel pie)
            pie.Alpha = ratio * 2 * Math.PI;
    }
    public ProjectBoardViewModel(Project project)
    {
        this.project = project;
        Selected = new CanvasNodesViewModel(project);
        if (Selected is CanvasNodesViewModel canvas)
        {
            canvas.OnUpdate += () =>
            {
                RefreshProgress();
            };
        }
        Graph = new graphViewModel();
        Piegraph =  new piegraphViewModel();
        Blockgraph = new blockGraphViewModel();

    }
}