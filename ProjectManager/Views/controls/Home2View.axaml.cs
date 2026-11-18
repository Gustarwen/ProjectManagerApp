using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Styling;
using ProjectManager.DataTemplates;
using ProjectManager.ViewModels;

namespace ProjectManager.Views;

public partial class Home2View : UserControl
{
    private void InitializeProjects()
    {
        foreach (var project in DataBaseController.Projectrepositroy.Query().Execute())
        {
            ProjectCardView projecrRepresenter = new ProjectCardView(project.Id,project.Name,project.ProjectBitmap);
            
        
            ProjectPanel.Children.Add(projecrRepresenter);
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
            Grid taskGrid = new Grid();
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            taskGrid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            CheckBox taskCheckBox = new CheckBox();
            taskCheckBox.IsChecked = task.Finished;
            taskViewModel vm = new taskViewModel();
            taskCheckBox.DataContext = vm;
            taskCheckBox.Command = vm.AcceptCommand;
            taskCheckBox.CommandParameter = task;
            Grid.SetColumn(taskCheckBox, 0);
            TextBlock taskName = new TextBlock();
            taskName.Text = task.Name;
            Grid.SetColumn(taskName, 1);
            TextBlock taskAccept = new TextBlock();
            taskAccept.Text = task.accept.Date.ToLongDateString();
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
        
    }
}