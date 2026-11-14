    using System;
    using System.Collections.Generic;
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

            List<DateTimeOffset> times = new();
            int d = 0;
            DateTimeOffset actuall;
            foreach (var task in tasks)
            {
                if (task.accept == null) continue;
               times.Add(task.accept);
            }

            List<int> data = new();
            actuall = times.Order().FirstOrDefault();
            foreach (var time in times.Order())
            {
                if (time > actuall)
                {
                    data.Add(d);
                    d = 0;
                    continue;
                }

                d++;
            }

            if (Graph is graphViewModel graph)
            {
                graph.datas.Clear();
                graph.datas.AddRange(data);
            }
            
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