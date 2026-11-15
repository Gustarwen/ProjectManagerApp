    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CommunityToolkit.Mvvm.ComponentModel;
    using ProjectManager.DataTemplates;


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

            var data = tasks
                .Where(t => t.Finished)
                .GroupBy(t => t.accept.Date)
                .OrderBy(g => g.Key)
                .Select(g => g.Count())
                .ToList();

            if (Graph is graphViewModel graph)
            {
                if (data == null) return;
                if (graph.Datas.Count == data.Count)
                {
                    bool ret = false;
                    for (int i = 0; i < data.Count; i++)
                    {
                        if (data[i] == graph.Datas[i])
                        {
                            continue;
                        }

                        ret = true;
                    }

                    if (ret == false)
                    {
                        return;
                    }
                }
                Console.WriteLine($"data: {data.Count} -> {string.Join(",", data)}");
                graph.Datas=data;
            }
            
        }
        
        
        public ProjectBoardViewModel(Project project)
        {
            if (Graph is graphViewModel graph)
            {
                graph.PropertyChanged += (e, v) => { RefreshProgress(); };
            }

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