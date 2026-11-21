using System;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using Avalonia.VisualTree;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;
using ProjectManager.Views.Windows;

namespace ProjectManager.Views;

public partial class Home2View : UserControl
{
    public void InitializeTasks()
    {
        TaskPanel.Children.Clear();
        foreach (var task in DataBaseController.Taskrepository.Query().Execute())
        {
            if (TaskPanel.Children.Count <= 4)
            {
                Border taskRepresenter = new Border()
                {
                    BorderThickness = new Thickness(2,0,0,0),
                    BorderBrush = Brush.Parse("#0047CC"),
                    
                };
                Grid grid = new Grid()
                {
                    VerticalAlignment = VerticalAlignment.Stretch
                };
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Auto));
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Star));
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Star));
                CheckBox check = new CheckBox()
                {
                    IsChecked = task.Finished
                };
                check.Click += (a, v) => {
                    task.Finished=!task.Finished;
                    if (task.Finished)
                    {
                        task.accept = DateTime.Now;
                    }
                    
                    
                    DataBaseController.Taskrepository.Update(task);
                    InitializeProjects();
                };
                Grid.SetColumn(check,0);
                grid.Children.Add(check);
                TextBlock name = new TextBlock();
                name.Text = task.Name;
                Grid.SetColumn(name,1);
                grid.Children.Add(name);
                
                TextBlock deadline = new TextBlock();
                deadline.Text = task.Date.ToString();
                grid.Children.Add(deadline);
                Grid.SetColumn(deadline,2);
                
                taskRepresenter.Child = grid;
                
            TaskPanel.Children.Add(taskRepresenter);
                
            }
        }
    }
    public void InitializeProjects()
    {
        ProjectPanel.Children.Clear();
        HomeViewModel viewmodel = new HomeViewModel();
   
        foreach (var project in DataBaseController.Projectrepositroy.Query().Execute())
        {
            ProjectCardView projecrRepresenter = new ProjectCardView(project.Id,project.Name,project.ProjectBitmap);

          Viewbox box = new Viewbox();
          box.Child = projecrRepresenter;
            ProjectPanel.Children.Add(box);

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


    public Home2View()
    {
        InitializeComponent();
        InitializeProjects();
        InitializeTasks();

        AddProject.Click += (a, v) =>
        {
            
            MainWindow mainWindow = this.FindAncestorOfType<MainWindow>();
            Window dialog = new ProjectAddView(this);
            dialog.ShowDialog(mainWindow);
            
            
         //tady vytvor novy dialog
        };

    }
}