using System;
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
        };
        
      



    }
}