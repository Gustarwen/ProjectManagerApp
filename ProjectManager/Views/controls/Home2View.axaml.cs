using System;
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
            };
        }
    }

    private void InitializeTasks()
    {
        /*<Border BorderBrush="#0077b6" BorderThickness=" 0 1 0 0">
        <Grid ColumnDefinitions="Auto * Auto">
        <CheckBox></CheckBox>
        <TextBlock VerticalAlignment="Center" Grid.Column="1">some random task</TextBlock>
        <TextBlock VerticalAlignment="Center" Grid.Column="2" Margin="0 0 10 0">pozitri</TextBlock>
        </Grid>
        </Border>*/
        foreach (var task in DataBaseController.Taskrepository.Query().Execute())
        {
            Border taskRepresenter = new Border();
            taskRepresenter.CornerRadius = new CornerRadius(0);
            Grid taskGrid = new Grid();
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            CheckBox taskCheckBox = new CheckBox();
            taskCheckBox.IsChecked = task.Finished;
            taskCheckBox.BorderBrush =Brushes.Black;
          ;

          taskCheckBox.Click += (a, v) =>
          {
                task.Finished = !task.Finished;
                task.accept = DateTime.Now;
                DataBaseController.Taskrepository.Update(task);
          };
            Grid.SetColumn(taskCheckBox, 0);
            TextBlock taskName = new TextBlock();
            taskName.Text = task.Name;
            taskName.Foreground = Brushes.Black;
            Grid.SetColumn(taskName, 1);
            TextBlock taskAccept = new TextBlock();
            taskAccept.Text = task.accept.Date.ToLongDateString();
            taskAccept.Foreground = Brushes.Black;
            Grid.SetColumn(taskAccept, 2);
            
            
            taskGrid.Children.Add(taskCheckBox);
            taskGrid.Children.Add(taskName);
            taskGrid.Children.Add(taskAccept);
            taskRepresenter.Child = taskGrid;
            taskRepresenter.BorderBrush = Brush.Parse("#0077b6");
            taskRepresenter.BorderThickness = new Thickness(0, 1, 0, 0);
            
            TaskPanel.Children.Add(taskRepresenter);
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