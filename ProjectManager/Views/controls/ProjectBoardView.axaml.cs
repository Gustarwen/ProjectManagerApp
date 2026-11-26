using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class ProjectBoardView : UserControl
{
    private double data; 
    public ProjectBoardView()
    {
        
        InitializeComponent();
        DataContextChanged += (a, v) =>
        {
            if (DataContext is not ProjectBoardViewModel) return;
            ProjectBoardViewModel vm = (ProjectBoardViewModel)this.DataContext;
            data = (double) DataBaseController.Taskrepository.Query().Where(p => p.Finished == true&& p.ProjectId==vm.project.Id).Execute().ToList().Count()/DataBaseController.Taskrepository.Query().Where(p => p.ProjectId==vm.project.Id).Execute().ToList().Count() *2 *Math.PI;
            if (vm.Piegraph is piegraphViewModel pie)
            {
                pie.Alpha = data;
            }
            if (vm.Blockgraph is blockGraphViewModel blockGraph)
            {
                var tasksOrderBY = DataBaseController.Taskrepository.Query().Where(p=>p.ProjectId==vm.project.Id).GroupBy(t => t.accept).Execute().ToList();

                    List<int> taskPerDate = new List<int>();
                    int datum = 0;
                    DateTimeOffset actuallDate = new DateTimeOffset();
                    foreach (var task in tasksOrderBY)
                    {
                        if (task.Key == actuallDate )
                        {
                            datum++;
                        }
                        else
                        {
                            actuallDate = task.Key;
                            taskPerDate.Add(datum);
                            datum = 0;
                        }
                    }
                    blockGraph.Datas = taskPerDate;
                
            }
        };
        
      



    }
}