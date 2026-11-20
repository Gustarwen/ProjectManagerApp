using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ProjectManager.DataTemplates;

namespace ProjectManager.Views.Windows;

public partial class ProjectAddView : Window
{
    public ProjectAddView(Home2View view)
    {
        InitializeComponent();
        Button.Click += (e, v) =>
        {
            if (ProjectName.Text == null || DeadLineDate.SelectedDate == null) return;
      
            Project project = new Project()
            {
                Name = ProjectName.Text,
                Date =  DeadLineDate.SelectedDate,
                Finished = false
            };
            Project withid =DataBaseController.Projectrepositroy.Create(project);
            ProjectData projectData = new ()
            {
                ProjectId = withid.Id,
                Name = ProjectName.Text,
                Date = DeadLineDate.SelectedDate,
                Finished = false
                
            };
            MainProjectData mainprojectData = new MainProjectData(projectData);
            DataBaseController.Taskrepository.Create(projectData);
            Console.WriteLine("projekt byl pridan");
           view.InitializeProjects();
            this.Close();
        };

    }
}