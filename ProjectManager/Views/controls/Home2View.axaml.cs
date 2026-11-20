using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
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
        foreach (var task in DataBaseController.Taskrepository.Query().Execute().OrderBy(p=>p.Date))
        {
            if (TaskPanel.Children.Count <= 4)
            {
                Border taskRepresenter = new Border()
                {
                    BorderThickness = new Thickness(2,0,0,0),
                    BorderBrush = Brush.Parse("#0047CC"),
                    
                };
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Auto));
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Star));
                grid.ColumnDefinitions.Add(new ColumnDefinition(1,GridUnitType.Star));
                CheckBox check = new CheckBox();
                Grid.SetColumn(check,0);
                TextBlock name = new TextBlock();
                name.Text = task.Name;
                Grid.SetColumn(name,1);
                
                TextBlock deadline = new TextBlock();
                deadline.Text = task.Date.ToString();
                Grid.SetColumn(deadline,2);
                
                taskRepresenter.Child = grid;
                
                grid.Children.Add(taskRepresenter);
                
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