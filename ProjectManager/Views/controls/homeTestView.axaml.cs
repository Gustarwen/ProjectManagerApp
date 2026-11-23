using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class homeTestView : UserControl
{
      public void InitializeTasks()
    {
        TasksView.TaskPanel.Children.Clear();
        foreach (var task in DataBaseController.Taskrepository.Query().Execute())
        {
            if (TasksView.TaskPanel.Children.Count <= 4)
            {
                TaskCardView taskRepresenter = new();
                taskRepresenter.Name.Text = task.Name;
                taskRepresenter.Finished.IsChecked = task.Finished;
                
            
                
            TasksView.TaskPanel.Children.Add(taskRepresenter);
                
            }
        }
    }
    public void InitializeProjects()
    {
        ProjectPanels.Children.Clear();
        HomeViewModel viewmodel = new HomeViewModel();
   
        foreach (var project in DataBaseController.Projectrepositroy.Query().Execute())
        {
            ProjectCardView projecrRepresenter = new ProjectCardView(project.Id,project.Name,project.ProjectBitmap);

            Viewbox box = new Viewbox();
            box.Child = projecrRepresenter;
            ProjectPanels.Children.Add(box);

            projecrRepresenter.Delete.Click +=(i,v)=>
            {
                DataBaseController.Projectrepositroy.Delete(project);
                foreach (var task in DataBaseController.Taskrepository.Query().Where(p=>p.ProjectId==project.Id).Execute())
                {
                    DataBaseController.Taskrepository.Delete(task);
                }
                InitializeProjects();
                InitializeTasks();

            };
        }
    }
    public homeTestView()
    {
        InitializeComponent();
        InitializeProjects();
        InitializeTasks();
        this.SizeChanged += (s, e) =>
        {
            if (Bounds.Width < 860)
            {
                Statistics.IsVisible = false;
                Grid.SetColumnSpan(TaskBorder,2);
            }
            else
            {
                Statistics.IsVisible = true;
                Grid.SetColumnSpan(TaskBorder,1);
            }
        };
    }
}